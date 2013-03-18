/*

Twitter Feed
by Zachary Kniebel
Copyright (C) 2013 

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

(function ($) {
    $.fn.twitterFeed = function (options) {
        var settings = $.extend(
        true, {
            count: 5,
            rawData: "", //unparsed JSON result of tweet request
            screen_name: "",
            tweetBodyClass: "",
            date: {
                show: true,
                cssClass: "feedItem",
                order: 0,
                prepend: "",
                append: ""
            },
            reply: {
                show: true,
                cssClass: "feedItem",
                order: 1,
                prepend: "",
                append: ""
            },
            retweet: {
                show: true,
                cssClass: "feedItem",
                order: 2,
                prepend: "",
                append: ""
            },
            favorite: {
                show: true,
                cssClass: "feedItem",
                order: 3,
                prepend: "",
                append: ""
            },
            callbackOnEach: false,
            callback: null
        },
        options);

        if (!settings.screen_name) {
            return;
        };

        var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var parseTweetDate = function (date) {
            var date = new Date(date);
            var now = new Date(Date.now());
            var dateDiff = now - date;

            //(max dateDiff value + 1) to display date as relative time in the corresponding unit
            var seconds = 60;
            var minutes = seconds * 60;
            var hours = minutes * 24;

            if (dateDiff < seconds) {
                return dateDiff + " seconds ago";
            } else if (dateDiff < minutes) {
                return dateDiff + " minutes ago";
            } else if (dateDiff < hours) {
                return dateDiff + " hours ago";
            } else {
                return months[date.getUTCMonth()] + " " + date.getUTCDay() + " '" + (date.getUTCFullYear() + "").substring(2);
            }
        };

        var optionalMarkup = function (rawTweet) {
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
                    date += " class='" + settings.date.cssClass + "'>" + parsedDate + "</a>";
                } else {
                    date += "<span class='" + settings.date.cssClass + "'>" + parsedDate + "</span>";
                }
                date += settings.date.append;

                orderedElements[settings.date.order] = date;
            }
            if (settings.reply.show) {
                reply += settings.reply.prepend;
                reply += "<a href='https://twitter.com/intent/tweet?in_reply_to=" + rawTweet.id_str + "' class='" + settings.reply.cssClass + "'>reply</a>";
                reply += settings.reply.append;

                orderedElements[settings.reply.order] = reply;
            }
            if (settings.retweet.show) {
                retweet += settings.retweet.prepend;
                retweet += "<a href='https://twitter.com/intent/retweet?tweet_id=" + rawTweet.id_str + "' class='" + settings.retweet.cssClass + "'>retweet</a>";
                retweet += settings.retweet.append;

                orderedElements[settings.retweet.order] = retweet;
            }
            if (settings.favorite.show) {
                favorite += settings.favorite.prepend;
                favorite += "<a href='https://twitter.com/intent/favorite?tweet_id=" + rawTweet.id_str + "' class='" + settings.favorite.cssClass + "'>favorite</a>";
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
            var urlPattern = new RegExp("(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\\.))+(([a-zA-Z0-9\\._-]+\\.[a-zA-Z]{2,6})|([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}))(/[a-zA-Z0-9\&%_\\./-~-]*)?", "gi");
            text = text.replace(urlPattern, "<a href='$&'>$&</a>");

            var handlePattern = new RegExp("@([A-Za-z0-9_]+)", "gi");
            text = text.replace(handlePattern, function (match) {
                return "<a href='https://twitter.com/" + match.substring(1) + "'>" + match + "</a>";
            });

            return text;
        };

        var $this = $(this);
        var numTargets = $this.length;
        for (var i = 0; i < numTargets; i++) {
            var ele = $this[i];

            /** UPDATED: Compatible with Twitter API 1.1 **/
            var jsonTweets = JSON.parse(settings.rawData);
            jsonTweets = jsonTweets.slice(0, settings.count);

            var numTweets = jsonTweets.length;
            for (var j = 0; j < numTweets; j++) {
                var rawTweet = jsonTweets[j];
                if (rawTweet.text !== undefined) {
                    var $tweet = $("<span class='" + settings.tweetBodyClass + "'>" + sanitizeTweet(rawTweet.text) + "</span>" + optionalMarkup(rawTweet));
                    $tweet.appendTo(ele);

                    if (settings.callbackOnEach && !! settings.callback) {
                        settings.callback();
                    }
                }
            }

            /** DEPRECATED: March 5th, 2013 - Twitter API upgrade to 1.1, Twitter API 1.0 shutdown **/
            /*var url = "http://api.twitter.com/1/statuses/user_timeline.json?callback=?&screen_name=";
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
            }); */
        }

        if (!settings.callbackOnEach && !! settings.callback) {
            settings.callback();
        }
    };
})(jQuery);