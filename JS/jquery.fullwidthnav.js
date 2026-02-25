/**
 * FullWIdthNav 1.0 (Requires the dimensions plug-in)
 *
 * Takes any navigation built from a "ul li a" setup and increases
 * each a's padding left/right so that the entire row of lis/as take
 * up the full space of their parent.
 *
 * Usage: jQuery('#navigation, ul.tabs').fullWidthNav();
 *
 * @class fullWidthNav
 *
 * Copyright (c) 2008 Andreas Lagerkvist (andreaslagerkvist.com)
 * Released under a GNU General Public License v3 (http://creativecommons.org/licenses/by/3.0/)
 *
 * Edited by Mike Davis on 1/2/2009 for YourWebPro.
 * NIC Systems Group, Inc.
 *
 */
jQuery.fn.fullWidthNav = function() {
        // Always return each...
        
        return this.each(function() {
                var ul = jQuery(this),
                        navWidth = ul.innerWidth(),
                        tabWidth = 0,
                        numTabs = 0,
                        diff = 0,
                        diffLeft = 0,
                        diffRight = 0,
                        lastDiff = 0,
                        lastDiffLeft = 0,
                        lastDiffRight = 0,
                        totalDiff = 0,
                        tabPaddingLeft = parseInt(jQuery('a', ul).css('paddingLeft').replace("px", ""), 0),
                        tabPaddingRight = parseInt(jQuery('a', ul).css('paddingRight').replace("px", ""), 0),
                        tabMarginRight = parseInt(jQuery('li', ul).css('marginRight').replace("px", ""), 0);
                        
                if (isNaN(tabPaddingLeft)) { tabPaddingLeft = 0; }
                if (isNaN(tabPaddingRight)) { tabPaddingRight = 0; }
                if (isNaN(tabMarginRight)) { tabMarginRight = 0; }
                
                // Get the width of all the tabs and the number of tabs
                jQuery('li', ul).each(function() {
                	if (this.tagName && this.tagName.toUpperCase() == "LI") {
	                        tabWidth += jQuery(this).outerWidth() + tabMarginRight;
	                        numTabs++;
                        }
                });

                // Make sure last li has no margin-right and subtract the last li's margin-right from the total width of the tabs		
		var marginRightDiff = parseInt(jQuery('li:last-child', ul).css('marginRight'), 0);
		if (isNaN(marginRightDiff)) { marginRightDiff = 0; }

                tabWidth -= marginRightDiff;
                jQuery('li:last-child', ul).css({marginRight: 0});

                // Calculate the difference between the tabWidth and the navWidth
                // and how much each tab needs to increase in width
                // Take care of fractionals and add them to the last li
                totalDiff = navWidth - tabWidth;

                // IF the tabWidth is actually larger than the navWidth the diff will be negative and
                // we need to use ceil rather than floor (floor(-14.33) == 15, ceil(-14.33) == 14 and that's what we want in this case...)
                if(totalDiff > 0) {
                        diff = Math.floor(totalDiff / numTabs);
                }
                else {
                        diff = Math.ceil(totalDiff / numTabs);
                }

                lastDiff = diff + totalDiff % numTabs;

                // We may need to have different padding left and right values depending
                // on fractional pixels again... bloody pain this!
                if(diff > 0) {
                        diffLeft = diffRight = Math.floor(diff / 2);
                }
                else {
                        diffLeft = diffRight = Math.ceil(diff / 2);
                }

                diffRight += diff % 2;

                if(lastDiff > 0) {
                        lastDiffLeft = lastDiffRight = Math.floor(lastDiff / 2);
                }
                else {
                        lastDiffLeft = lastDiffRight = Math.ceil(lastDiff / 2);
                }

                lastDiffRight += lastDiff % 2;

        //      console.log('Navigation\'s width is ' +navWidth +'px, the ' +numTabs +' tabs in this nav takes up a total of ' +tabWidth +'px width (' +tabMarginRight +'px margin-right included) need to increase total tab width with ' +totalDiff +'px (' +diff +'px per tab and ' +lastDiff +'px on the last tab)');

                // Now increae every tab's link's left/right padding to make them take up all available space
                jQuery('li', ul).each(function(i) {
                        var li = jQuery(this),
                                pl = diffLeft + tabPaddingLeft,
                                pr = diffRight + tabPaddingRight;

                        // If last tab...
                        if(i === (numTabs-1)) {
                                pl = lastDiffLeft + tabPaddingLeft;
                                pr = lastDiffRight + tabPaddingRight;
                        }

                        // Negative padding is not allowed...
                        if(pl < 0) {
                                pl = 0;
                        }
                        if(pr < 0) {
                                pr = 0;
                        }
                        jQuery('a', li).css({paddingLeft: pl +'px', paddingRight: pr +'px'});
                });
        });
};