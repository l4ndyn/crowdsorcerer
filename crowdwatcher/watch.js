const axios_ = require('axios');

const login = require('facebook-chat-api');
const typeLocator = require('./typeLocator.js');
const processors = require('./processors.js');

let appState;
try {
	appState = require('./auth/auth.json');
} catch {
	console.log('App state not found. Crowdwatcher will terminate.');
}

const axios = axios_.create({
    baseURL: 'http://localhost:12234/',
    timeout: 1000
});

const typeToEndpoint = (type) => type + 's';

const postBody = (type, body) => {
    const endpoint = typeToEndpoint(type);
    if (!endpoint) throw 'Invalid type: ' + type;

    return axios.post('/' + endpoint, JSON.stringify(body), { 
        headers: {
            'Content-Type': 'application/json'
        }
    });
}

const processEvent = async (event) => {
    console.log(event);
    const type = typeLocator.getEventType(event);
    console.log('Type: ' + type);

    if (type == 'unknown') return;
    const body = processors.process(type, event);

    if (event.type == 'message') {
        body.messageId = event.messageID;
    }

    console.log('Body: ' + JSON.stringify(body));
    console.log();

    await postBody(type, body);
};

module.exports = {
	watch: async function() {
		if (appState === undefined) {
			return;
		}

		login({ appState: appState }, (err, api) => {
			if (err) return console.error(err);

			api.setOptions({ listenEvents: true });
			api.setOptions({ selfListen: true });

			console.log(`Watch has started at ${new Date()}.`);
			api.listenMqtt(async (err, event) => {
				if(err) return console.error(err);

                await processEvent(event)
                .catch(error => console.log(error));
			});
		});
	}
};