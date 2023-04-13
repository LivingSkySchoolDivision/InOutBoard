/* ******************************************** */
/* * Document onLoad stuff                    * */
/* ******************************************** */
$(document).ready(function(){
	onPageLoad();
});

/* ******************************************** */
/* * Interval stuff                           * */
/* ******************************************** */

/*
 1000     1 second
 10000     10 seconds
 60000     1 minute
 300000     5 minutes
 600000     10 minutes
 1800000     30 mins
 3600000     1 hour
 */

 setInterval(function() {
	onInterval_UpdateUserStatuses();
}, 10000);

setInterval(function() {
    window.location.replace(window.location.href);
}, 2700000);