if (typeof window.beast !== 'object') window.beast = {};
if (typeof window.beast.net !== 'object') window.beast.net = {};

beast.net.SignalR = function (options) {
    this.init(options);
};
beast.net.SignalR.prototype = {
    connection: null,
    url: '',

    init: function (options) {
        $.extend(this, options);

        this.connection = $.connection(this.url);
        this.connection.received($.proxy(this._onreceived, this));
        this.connection.error($.proxy(this._onerror, this));
    },

    start: function () {
        this.connection.start().done($.proxy(this._onconnected, this));
    },

    send: function (data) {
        this.connection.send(data);
    },

    _onreceived: function (data) {
        this.received(data);
    },
    _onerror: function (e) {
        if ((e.readyState && e.status) && (e.readyState === 4 && e.status === 200)) {
            //console.log(JSON.stringify(e));
            return;
        }
        this.error(e);
    },
    _onconnected: function () {
        this.connected();
    },

    received: function (data) { },
    error: function (e) { },
    connected: function () { }
};