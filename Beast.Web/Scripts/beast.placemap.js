(function ($) {
	var canvas, ctx;
	var settings = {
		width: 100,
		height: 100,
		exitcolor: '#C38312',
		tile: {
			real: 0,
			size: 0,
			padding: 0
		}
	};
	var _map = {
		init: function (options) {
			$.extend(settings, options);

			settings.tile.size = settings.tile.real + (settings.tile.padding * 2);

			canvas = this[0];
			canvas.width = settings.width;
			canvas.height = settings.height;

			ctx = canvas.getContext('2d');
			_map.draw();

			return this;
		},
		draw: function () {
			ctx.fillStyle = '#000';
			ctx.fillRect(0, 0, canvas.width, canvas.height);


			return this;
		},
		_drawExit: function (ctx, p1, p2) {
			ctx.beginPath();
			ctx.moveTo(p1.x, p1.y);
			ctx.lineTo(p2.x, p2.y);
			ctx.closePath();

			ctx.strokeStyle = settings.exitcolor;
			ctx.lineWidth = 3;
			ctx.stroke();
		}
	};
	$.fn.placemap = function (method) {
		if (_map[method]) {
			return _map[method].apply(this, Array.prototype.slice.call(arguments, 1));
		} else if (typeof method === 'object' || !method) {
			return _map.init.apply(this, arguments);
		} else {
			$.error('Method ' + method + ' does not exist on jQuery.map');
		}
	};
})(jQuery);