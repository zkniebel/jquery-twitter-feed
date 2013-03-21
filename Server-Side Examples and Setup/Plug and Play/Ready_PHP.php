<?php

$twitter_handle = "UPDATE ME";

function buildBaseString($baseURI, $method, $params) {
    $r = array();
    ksort($params);
    foreach($params as $key=>$value){
        $r[] = "$key=" . rawurlencode($value);
    }
    return $method."&" . rawurlencode($baseURI) . '&' . rawurlencode(implode('&', $r));
}

function buildAuthorizationHeader($oauth) {
    $r = 'Authorization: OAuth ';
    $values = array();
    foreach($oauth as $key=>$value)
        $values[] = "$key=\"" . rawurlencode($value) . "\"";
    $r .= implode(', ', $values);
    return $r;
}

$url = "https://api.twitter.com/1.1/statuses/user_timeline.json";

$oauth_access_token = "UPDATE ME";
$oauth_access_token_secret = "UPDATE ME";
$consumer_key = "UPDATE ME";
$consumer_secret = "UPDATE ME";

$oauth = array( 'oauth_consumer_key' => $consumer_key,
                'oauth_nonce' => time(),
                'oauth_signature_method' => 'HMAC-SHA1',
                'oauth_token' => $oauth_access_token,
                'oauth_timestamp' => time(),
                'oauth_version' => '1.0',
				'screen_name' => $twitter_handle);

$base_info = buildBaseString($url, 'GET', $oauth);
$composite_key = rawurlencode($consumer_secret) . '&' . rawurlencode($oauth_access_token_secret);
$oauth_signature = base64_encode(hash_hmac('sha1', $base_info, $composite_key, true));
$oauth['oauth_signature'] = $oauth_signature;

// Make Requests
$header = array(buildAuthorizationHeader($oauth), 'Content-Type: application/json', 'Expect:');
$options = array( CURLOPT_HTTPHEADER => $header,
                  //CURLOPT_POSTFIELDS => $postfields,
                  CURLOPT_HEADER => false,
                  CURLOPT_URL => $url . '?screen_name=' . $twitter_handle,
                  CURLOPT_RETURNTRANSFER => true,
                  CURLOPT_SSL_VERIFYPEER => false);
$feed = curl_init();
curl_setopt_array($feed, $options);
$json = curl_exec($feed);
curl_close($feed);

?>

<!DOCTYPE html>
<html>
<head>
<title>Twitter Live PHP Testing</title>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
<script type="text/javascript" src="https://raw.github.com/zkniebel/jquery-twitter-feed/master/jquery-twitter-feed.js"></script>
<script type="text/javascript">
	$(document).ready(function() {
		$(".homeTwitterFeed").each(function() {
			var $this = $(this);
			var $hfTwitter = $this.siblings(".hfTwitter");
			var screenname = '<?php echo $twitter_handle; ?>';
			var rawJSON = '<?php echo $json; ?>';
			if (screenname && rawJSON) {
				$this.twitterFeed({
					count: 8,
					rawData: rawJSON,
					screen_name: screenname,
					tweetBodyClass: "tweetBody homeFeedBody",
					date: { prepend: "<div class='homeFeedLinks'>", append: " - ", cssClass: "feedItemDate" },
					reply: { append: " - ", },
					favorite: { prepend: " - ", append: "</div>"}
				});
			} else {
				$this.css("display","none");
			}	
		});
	});
</script>
</head>
<body>
<div class="homeTwitterFeed">
</div>
</body>
</html>










