/*

Twitter Feed
by Zachary Kniebel
Copyright (C) 2013 

Twitter Feed is a robust and highly customizable jQuery Twitter plugin for 
retrieving the latest tweets of a specified Twitter user. The plugin 
provides support reply, retweet and favorite links, as well as the date of
the tweet. 


Features:
   - Ability to show/hide Reply, Retweet and Favorite (all shown by 
     default)
   - Ability to show/hide Date of tweet - can also be set to link to the 
     tweet (shown and rendered as link by default)
   - Ability to supply markup to be inserted be inserted before or after 
     any optional links/date
   - Ability to set the order in which optional links will/date be 
     displayed
   - Ability to supply a CSS class to any text/markup rendered, including 
     the tweet body and all optional links/date
   - Ability to specify callback function to be called either when the 
     function completes, or when each tweet is rendered
     - note that any callbacks called when the fuction completes will be 
       called before the tweet is done loading (AJAX)
   - Ability to specify number of tweets to display
   

Twitter Feed Options Object
   - count            : the number of tweets to display
   - screen_name      : the Twitter handle from which to retrieve the 
                        tweets (do not include the '@' symbol)
   - tweetBodyClass   : a CSS class applied to the body of each retrieved 
                        tweet
   - date             : an object representing optional link settings (see 
                        Dates and Optional Link Settings)
   - reply            : an object representing optional link settings (see 
                        Optional Link Settings)
   - retweet          : an object representing optional link settings (see 
                        Optional Link Settings)
   - favorite         : an object representing optional link settings (see 
                        Optional Link Settings)
   - callbackOnEach   : if true, the supplied callback function will be 
                        called after each tweet is loaded
   - callback         : an optional callback function to be called after 
                        each or all tweet(s) have loaded


 Dates
   The plugin's included parseTweetDate function parses dates based based 
   on when the tweet was created, similar to the way Twitter does on their 
   own site. The date-ranges in the below table  represent the time the 
   tweet was created, relative to the current time. These ranges are used 
   as conditional rules for parsing the dates in their corresponding 
   formats. 

   |      Time Since Tweeting        |       Parsed Example       |
   ----------------------------------------------------------------
   |      1 second - 59 seconds      |       43 seconds ago       |
   |      1 minute - 59 minutes      |       11 minutes ago       |
   |      1 hour   - 23 hours        |       7 hours ago          |
   |      1 day    - XXXXXXXXXX      |       Feb 23 '12           |

   If these formats are not to your liking, you need only replace the 
   parseTweetDate functoin with one of your own.


 - Optional Link Settings (also applies to date)
   Optional link settings are supplied in object literal form. All of the 
   objects representing optional 
   link settings have the following properties:
   - show     : if set to true, the link/date is shown (shown by default)
   - class    : CSS class to be applied to the link/date
   - order    : number (zero-based index) indicating when link should be 
                rendered in ordering
   - prepend  : HTML markup to be inserted before the link/date
   - append   : HTML markup to be inserted after the link/date
   - link     : indicates whether or not the date should link to the tweet 
                (note: effective on date)



--------------------------------------------------------------------------

This software is protected under the MIT license, below, and under the 
Open Source GPL v3.0 license (http://opensource.org/licenses/GPL-3.0).


Copyright (C) 2013 Zachary Kniebel

Permission is hereby granted, free of charge, to any person obtaining a 
copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation
the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the 
Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.

*/

(function($){
    $.fn.twitterFeed = function (options) {		
		options.date = $.extend({ 
			show: true, 
			class: "feedItemDate", 
			order: 0,
			prepend: "",
			append: "",
			link: true,
		}, options.date);
		
		options.reply = $.extend({ 
			show: true, 
			class: "feedItem", 
			order: 1,
			prepend: "",
			append: "",
		}, options.reply);
		
		options.retweet = $.extend({ 
			show: true, 
			class: "feedItem", 
			order: 2,
			prepend: "",
			append: "",
		}, options.retweet);
		
		options.favorite = $.extend({ 
			show: true, 
			class: "feedItem", 
			order: 3,
			prepend: "",
			append: "",
		}, options.favorite);
		
        var settings = $.extend({
            count: 5,
            screen_name: "",     
			tweetBodyClass: "",
			callbackOnEach: false,
			callback: null
        }, options);
        
        if (!settings.screen_name) { return; };

		var months = [ "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" ];
		var parseTweetDate = function(date) {
			var date = new Date(date);
			var now = new Date(Date.now());
			var sameDay = 
				date.getFullYear() == now.getFullYear() &&
				date.getMonth() == now.getMonth() &&
				date.getDay() == now.getDay();
			
			if (sameDay) {
				if (date.getHours() == now.getHours()) {
					if (date.getMinutes() == now.getMinutes()) {
						return now.getSeconds() - date.getSeconds() + " seconds ago";
					} else {
						return now.getMinutes() - date.getMinutes() + " minutes ago";
					}
				} else {
					return now.getHours() - date.getHours() + " hours ago";
				}
			} else {
				return months[date.getMonth()] + " " + date.getDay() + " '" + (date.getFullYear() + "").substring(2);
			}
		};
		
		var optionalMarkup = function(rawTweet) {	
			var date = "";
			var reply = "";
			var retweet = "";
			var favorite = "";
			var orderedElements = [];
			
			if (settings.date.show && rawTweet.created_at) {
				date += settings.date.prepend;
				var parsedDate = parseTweetDate(rawTweet.created_at);				
				if (settings.date.link) {
					date += "<a href='https://twitter.com/" + settings.screen_name + "/status/" + rawTweet.id_str + "'";
					date += " class='" + settings.date.class + "'>" + parsedDate + "</a>";
				} else {
					date += "<span class='" + settings.date.class + "'>" + parsedDate + "</span>";
				}
				date += settings.date.append;
				
				orderedElements[settings.date.order] = date;
			}
			if (settings.reply.show) {
				reply += settings.reply.prepend;
				reply += "<a href='https://twitter.com/intent/tweet?in_reply_to=" + rawTweet.id_str + "' class='" + settings.reply.class + "'>reply</a>";
				reply += settings.reply.append;
				
				orderedElements[settings.reply.order] = reply;
			}
			if (settings.retweet.show) {
				retweet += settings.retweet.prepend;
				retweet += "<a href='https://twitter.com/intent/retweet?tweet_id=" + rawTweet.id_str + "' class='" + settings.retweet.class + "'>retweet</a>";
				retweet += settings.retweet.append;
				
				orderedElements[settings.retweet.order] = retweet;
			}
			if (settings.favorite.show) {
				favorite += settings.favorite.prepend;
				favorite += "<a href='https://twitter.com/intent/favorite?tweet_id=" + rawTweet.id_str + "' class='" + settings.favorite.class + "'>favorite</a>";
				favorite += settings.favorite.append;
				
				orderedElements[settings.favorite.order] = favorite;
			}
							
			var markup = "";
			for (var i = 0; i < orderedElements.length; i++) {
				if (orderedElements[i]) {
					markup += orderedElements[i];
				}
			}	
			
			return markup;
		};
		
		function sanitizeTweet(text) {
			var urlPattern = new RegExp("(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\\.))+(([a-zA-Z0-9\\._-]+\\.[a-zA-Z]{2,6})|([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}))(/[a-zA-Z0-9\&%_\\./-~-]*)?","gi");
			text = text.replace(urlPattern, "<a href='$&'>$&</a>");
			
			var handlePattern = new RegExp("@([A-Za-z0-9_]+)", "gi");
			text = text.replace(handlePattern, function(match) { return "<a href='https://twitter.com/" + match.substring(1) + "'>" + match + "</a>"; });
			
			return text;
		};

        this.each(function () {
            var ele = $(this);
            var url = "http://api.twitter.com/1/statuses/user_timeline.json?callback=?&screen_name=";
            url += settings.screen_name;
            url += "&count=" + settings.count;
            
            $.ajax({
                cache: false
            });

            $.getJSON(url, function (data) {
                $.each(data, function (i, rawTweet) {
                    if (rawTweet.text !== undefined) {
						var $tweet = $("<span class='" + settings.tweetBodyClass + "'>" + sanitizeTweet(rawTweet.text) + "</span>" + optionalMarkup(rawTweet));
                        $tweet.appendTo(ele);
											
						if (settings.callbackOnEach && !!settings.callback) {
							settings.callback();
						}
                    }
                });
            });
        });

        if (!settings.callbackOnEach && !!settings.callback) {
            settings.callback();
        }
    };
})(jQuery);