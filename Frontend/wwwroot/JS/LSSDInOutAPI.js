var INOUTAPIRoot = "https://inoutapi.lskysd.ca"; // No trailing slash

function log(string) {
	console.log(string);
}

/* ************************************************************** */
/* * AJAX / JSON Logic                                          * */
/* ************************************************************** */

function INOUTAPIGetUserStatus(personID, callbackFunction) {
	$.getJSON(INOUTAPIRoot + "/People/" + personID, function(data) {
		callbackFunction(data);
	}); // getJSON
}

function INOUTAPIGetAllUsers(callbackFunction) {
	var JSONURL = INOUTAPIRoot + "/People/"
	log("Getting all users from " + JSONURL);
	var returnMe = new Array();
	$.getJSON(JSONURL, function(data) {
		$.each(data, function(categoryIndex, person) {
			returnMe.push(person);
		}); // each

		log ("Got response - returning " + returnMe.length +  " users");
		callbackFunction(returnMe);
	}); // getJSON
}

function INOUTAPIGetGroupMembers(groupID,callbackFunction) {
	if ((groupID == 0) || (groupID == null)) {
		INOUTAPIGetAllUsers(callbackFunction);
		return;
	}

	var JSONURL = INOUTAPIRoot + "/GroupMembers/" + groupID
	log("Getting all users from " + JSONURL);
	var returnMe = new Array();
	$.getJSON(JSONURL, function(data) {
		$.each(data, function(categoryIndex, person) {
			returnMe.push(person);
		}); // each

		log ("Got response - returning " + returnMe.length +  " users");
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

function INOUTAPIGetAllGroups(callbackFunction) {
	var JSONURL = INOUTAPIRoot + "/Groups/";
	var returnMe = new Array();
	$.getJSON(JSONURL, function(data) {
		$.each(data, function(categoryIndex, group) {
			returnMe.push(group);
		}); // each
		log("Returning " + returnMe.length + " groups");
		callbackFunction(returnMe);
	}); // getJSON
}

function INOUTAPIGetGroup(groupID, callbackFunction) {
	var JSONURL = INOUTAPIRoot + "/Groups/" + groupID;
	$.getJSON(JSONURL, function(data) {

		callbackFunction(data);
	}); // getJSON
}
