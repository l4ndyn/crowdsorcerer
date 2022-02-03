const fs = require("fs");
const login = require("facebook-chat-api");

let credentials;
try {
	credentials = require('./auth/credentials.json');
} catch {
	console.log('Credentials not found. Crowdwatcher will terminate.');
}

login(credentials, (err, api) => {
    if(err) return console.error(err);

    fs.writeFileSync('appstate.json', JSON.stringify(api.getAppState()));
});