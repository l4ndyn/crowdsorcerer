module.exports = {
    process: function(type, event) {
        switch (type) {
            case 'text':
                return this.processText(event);
            default:
                return undefined;
        }
    },
    
    processText: function(event) {
        return {
            text: event.body
        };
    }
}