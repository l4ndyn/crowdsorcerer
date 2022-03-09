function toUrl(string) {
    let url;
    
    try {
        url = new URL(string);
    } catch (_) {
        return undefined;
    }
  
    if (url.protocol === "http:" || url.protocol === "https:") {
        return url;
    }
    
    return undefined;
}

const ytTitleMustStartWith = ['yt ', 'yt: ', 'yt:', 'youtube ', 'youtube: ', 'youtube:', 'play: ', 'play:', 'play ',
                                'Yt ', 'Yt: ', 'Yt:', 'Youtube ', 'Youtube: ', 'Youtube:', 'Play: ', 'Play:', 'Play '];
function isYoutubeTitleIdentifier(message) {
    
    return ytTitleMustStartWith.map(s => message.startsWith(s)).reduce((a, c) => a || c);
}

module.exports = {
    ytTitleMustStartWith: ytTitleMustStartWith,
    getEventType: function(event) {
        let type = 'unknown';
    
        if (event.type == 'message') {
            if (event.attachments.length == 0) {
                type = 'text';

                const url = toUrl(event.body);
                if (url) {
                    if (((url.origin == 'https://youtube.com' || url.origin == 'https://www.youtube.com')
                     && (url.pathname == '/watch' || url.pathname == '/playlist')) || url.origin == 'https://youtu.be') {
                        type = 'youtubeUrl';
                    } else if (url.origin == 'https://open.spotify.com' && (url.pathname.startsWith('/track/') || url.pathname.startsWith('/playlist/'))) {
                        type = 'spotifyUrl';
                    }
                }

                if (isYoutubeTitleIdentifier(event.body)) {
                    type = 'youtubeTitle';
                }
            } else if (event.attachments.length == 1) {
                const at = event.attachments[0];
                if (at.type == 'photo') {
                    type = 'image';
                } else if (at.type == 'video') {
                    type = 'video';
                } else if (at.type == 'audio') {
                    type = 'voiceMessage';
                }
            }
        } else if (event.type == 'message_reaction') {
            if (event.reaction) type = 'reaction';
            else type = 'removedReaction';
        }
    
        return type;
    }
};