// author: Bas Wenneker		website: http://www.solutoire.com
// Fx.FontPlus is MIT-Licensed

Fx.FontPlus = new Class({
    initialize: function(elements, sid, gid, gmid, growsize, growmoresize) {
        this.growsize = (growsize) ? growsize : 2;
        this.growmoresize = (growmoresize) ? growmoresize : 4;

        this.elements = [];
        elements.each(function(el) {
            this.elements.push([el, el.getStyle('font-size').toInt()]);
        }, this);

        $(gmid).onclick = function() { this.growmore() } .bind(this);
        $(gid).onclick = function() { this.grow() } .bind(this);
        $(sid).onclick = function() { this.shrink() } .bind(this);
    },

    growmore: function() {
        this.elements.each(function(el) {
            var currentfsize = el[0].getStyle('font-size').toInt();
            if (currentfsize == el[1])
                el[0].effect('font-size', { duration: 300, unit: 'px' }).custom(el[1], el[1] + this.growmoresize);
            else if (currentfsize == el[1] + this.growsize)
                el[0].effect('font-size', { duration: 300, unit: 'px' }).custom(el[1] + this.growsize, el[1] + this.growmoresize);
        }, this);
    },

    grow: function() {
        this.elements.each(function(el) {
            var currentfsize = el[0].getStyle('font-size').toInt();
            if (currentfsize == el[1])
                el[0].effect('font-size', { duration: 300, unit: 'px' }).custom(el[1], el[1] + this.growsize);
            else if (currentfsize == el[1] + this.growmoresize)
                el[0].effect('font-size', { duration: 300, unit: 'px' }).custom(el[1] + this.growmoresize, el[1] + this.growsize);
        }, this);
    },

    shrink: function() {
        this.elements.each(function(el) {
            var currentfsize = el[0].getStyle('font-size').toInt();
            if (currentfsize == el[1] + this.growsize)
                el[0].effect('font-size', { duration: 300, unit: 'px' }).custom(el[1] + this.growsize, el[1]);
            else if (currentfsize == el[1] + this.growmoresize)
                el[0].effect('font-size', { duration: 300, unit: 'px' }).custom(el[1] + this.growmoresize, el[1]);
        }, this);
    }
});
