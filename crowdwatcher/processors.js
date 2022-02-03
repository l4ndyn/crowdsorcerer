module.exports = {
    process: function(type, event) {
        switch (type) {
            case 'text':
                return this.processText(event);
            case 'image':
                return this.processImage(event);
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
            imageUrl: event.attachments[0].largePreviewUrl;
        };
    }
}