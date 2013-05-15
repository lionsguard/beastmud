if (typeof window.beast !== 'object') window.beast = {};

beast.Console = function (options) {
    this.init(options);
};
beast.Console.prototype = {
    container: null,
    css: [],
    debug: false,

    init: function (options) {
        $.extend(this, options);
    },

    write: function (type, text) {
        if (!this.container) {
            console.log('beast.Console.container is not defined.');
            return;
        }

        if (type == beast.Notifications.DEBUG && !this.debug)
            return;

        if (!((typeof text) == 'string')) {
            text = text.toString();
        }

        var div = $('<div></div>').attr('class', this.css[type]).html(text);
        this.container.append(div);
        this.container.scrollTop(this.container[0].scrollHeight);
    }
};