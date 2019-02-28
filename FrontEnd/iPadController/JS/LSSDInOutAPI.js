var INOUTAPIRoot = "https://inoutapi.lskysd.ca/api/";

function log(string) {	
	console.log(string);	
}

/* ************************************************************** */
/* * AJAX / JSON Logic                                          * */
/* ************************************************************** */


function INOUTAPIGetUserStatus(personID, callbackFunction) {		
	$.getJSON(INOUTAPIRoot + "/PeopleWithStatus/" + personID, function(data) {	
		callbackFunction(data);
	}); // getJSON
}

function INOUTAPIGetAllUsers(callbackFunction) {
	var JSONURL = INOUTAPIRoot + "/PeopleWithStatus/"
	log("Getting all users from " + JSONURL);
	var returnMe = new Array();	
	$.getJSON(JSONURL, function(data) {	
		$.each(data, function(categoryIndex, person) {
			returnMe.push(person);
		}); // each

		log ("Got response - returning " + returnMe.count +  " users");	
		callbackFunction(returnMe);
	}); // getJSON	
}

function INOUTAPIPostStatus(personID, status, callbackFunction) {
	log("Posting new status for " + personID);

	$.ajax({
		url:INOUTAPIRoot + '/status/',
		method:'POST',			
		dataType:'json',
		data:JSON.stringify(status),
		contentType:"application/json",				
		success: function() {
			callbackFunction(personID, status);
			log("Posted new status for person " + personID + " successfully");
		},
		error: function(data, status, xhr) {
			log("Failed to set new status for person " + personID + "");
			log("Error:" + data);
		}
	});
}
