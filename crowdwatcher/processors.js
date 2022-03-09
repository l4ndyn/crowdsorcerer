const typeLocator = require('./typeLocator.js');
const messageTypes = ['text', 'image', 'video', 'youtubeUrl'];

module.exports = {
    process: function(type, event) {
        switch (type) {
            case 'text':
                return this.processText(event);
            case 'image':
                return this.processImage(event);
            case 'video':
                return this.processVideo(event);
            case 'youtubeUrl':
                return this.processUrl(event);
            case 'spotifyUrl':
                return this.processUrl(event);
            case 'voiceMessage':
                return this.processAudio(event);
            case 'reaction':
                return this.processReaction(event);
            case 'removedReaction':
                return this.processReaction(event);
            case 'youtubeTitle':
                return this.processYoutubeTitle(event);
            default:
                return undefined;
        }
    },
    
    processText: function(event) {
        return {
            text: event.body
        };
    },
    processYoutubeTitle: function(event) {
        let body;
        for (const identifier of typeLocator.mustStartWith) {
            if (event.body.startsWith(identifier)) {
                body = event.body.slice(identifier.length);
                break;
            }
        }
        
        return {
            text: body
        };
    },
    processImage: function(event) {
        return {
            description: event.body,
            imageUrl: event.attachments[0].largePreviewUrl
        };
    },
    processVideo: function(event) {
        return {
            description: event.body,
            videoUrl: event.attachments[0].url
        };
    },
    processUrl: function(event) {
        return {
            url: event.body
        };
    },
    processAudio: function(event) {
        return {
            description: event.body,
            audioUrl: event.attachments[0].url
        };
    },
    processReaction: function(event) {
        return {
            targetMessageId: event.messageID
        };
    }
}