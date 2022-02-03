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
            default:
                return undefined;
        }
    },
    
    processText: function(event) {
        return {
            text: event.body
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
}