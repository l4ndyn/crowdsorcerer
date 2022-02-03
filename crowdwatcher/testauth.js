/* eslint-disable no-inline-comments */
/* eslint-disable no-prototype-builtins */
/* eslint-disable no-case-declarations */

// const fs = require('fs');
const login = require('facebook-chat-api');

let appState;
try {
	appState = require('./auth.json');
}
catch {
	console.log('[Sentinel] App state not found. Sentinel will terminate.');
}

login({ appState: appState }, (err, api) => {
    if(err) return console.error(err);
    console.log('jaj');
});

let stopped = false;

module.exports = {
	watch: function(callback_msg, callback_photo, callback_reply) {
		if (appState === undefined) {
			return;
		}
		stopped = false;

		login({ appState: appState }, (err, api) => {
			if(err) return console.error(err);

			api.setOptions({ listenEvents: true });
			api.setOptions({ selfListen: true });

			console.log(`[Sentinel] Watch has started at ${new Date()}.`);
			const stopListening = api.listenMqtt((err, event) => {
				if(err) return console.error(err);

				if (stopped) {
					return stopListening();
				}

				if (event.threadID == '2562332187160337') {
					switch(event.type) {
					case 'message':
						getContent(event, (user, msg, isPhoto) => {
							if (user !== undefined) {
								if (!isPhoto) callback_msg(user, msg);
								else callback_photo(user, msg);
							}
						});

						break;
					case 'message_reply':
						getContent(event, (user, msg, isPhoto) => {
							if (user !== undefined) {
								getContent(event.messageReply, (r_user, r_msg, r_isPhoto) => {
									if (r_user !== undefined) {
										if (!r_isPhoto) callback_msg(r_user, r_msg, true);
										else callback_photo(r_user, r_msg, true);

										if (!isPhoto) callback_msg(user, msg, false);
										else callback_photo(user, msg, false);
									}
								});
							}
						});

						break;
					case 'event':
						console.log(event);
						break;
					}
				}
			});

			function getContent(event, callback) {
				let msg = event.body;
				let isPhoto = false;
				if (event.attachments.length == 1 && event.attachments[0].type == 'photo') {
					isPhoto = true;
					msg = event.attachments[0].largePreviewUrl;
				}

				let user = 'unknown';
				api.getUserInfo(event.senderID, (err, ret) => {
					if(err) return console.error(err);

					for(const prop in ret) {
						if(ret.hasOwnProperty(prop)) {
							user = ret[prop].name;
							callback(user, msg, isPhoto);
						}
					}
				});
			}
		});
	},
	stop: function() {
		if (stopped || appState === undefined) return;

		stopped = true;
		console.log(`[Sentinel] Watch has stopped at ${new Date()}.`);
	},
	isStopped: function() {
		return stopped;
	},
};