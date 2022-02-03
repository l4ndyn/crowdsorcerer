const axios_ = require('axios');

const login = require('facebook-chat-api');
const processors = require('./processors.js')

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

const typeToEndpoint = {
    text: "texts",
    image: "images"
};

const postBody = (type, body) => {
    const endpoint = typeToEndpoint[type];
    if (!endpoint) throw 'Invalid type: ' + type;

    return axios.post('/' + endpoint, JSON.stringify(body), { 
        headers: {
            'Content-Type': 'application/json'
        }
    });
}

const getEventType = (event) => {
    let type = 'unknown';

    if (event.type == 'message') {
        if (event.attachments.length == 0) {
            type = 'text';
        } else if (event.attachments.length == 1 && event.attachments[0].type == 'photo') {
            type = 'image';
        }
    }

    return type;
}

const processEvent = async (event) => {
    const type = getEventType(event);

    if (type == 'unknown') return;
    const body = processors.process(type, event);

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

			console.log(`[Crowdwatcher] Watch has started at ${new Date()}.`);
			api.listenMqtt(async (err, event) => {
				if(err) return console.error(err);

                await processEvent(event)
                .catch(error => console.log(error));
			});
		});
	}
};