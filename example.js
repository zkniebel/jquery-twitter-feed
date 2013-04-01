$(document).ready(function () {
        $("#feedTarget").twitterFeed({
		        count: 4,
                rawData: yourRawJSONData,
                prepend: "<div class='tweetWrapper'>", 
                append: "</div>",
                tweetBodyClass: "tweetBody tweetText",
		        date: { prepend: "<div>", append: " - ", order: 3, cssClass: "tweetDate" },
		        retweet: { show: false },
		        favorite: { prepend: " - ", order: 0, append: "</div>" },
		        callbackOnEach: true,
		        callback: function() { 
                $(this).find(".tweetBody").myCallbackOnEachTweet(); 
            }
	      });
    });
});