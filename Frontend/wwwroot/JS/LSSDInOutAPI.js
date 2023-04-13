//var INOUTAPIRoot = "https://inoutapi.lskysd.ca"; // No trailing slash
var INOUTAPIRoot = "https://localhost:5001"; // No trailing slash

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

function INOUTAPIGetGroupMembers(groupID, callbackFunction) {
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
		callbackFunction(returnMe);
	}); // getJSON
}

function INOUTAPIGetGroup(groupID, callbackFunction) {
	var JSONURL = INOUTAPIRoot + "/Groups/" + groupID;
	$.getJSON(JSONURL, function(data) {

		callbackFunction(data);
	}); // getJSON
}

function INOUTAPIGetUser(personid, callbackFunction) {
	var JSONURL = INOUTAPIRoot + "/People/" + personid

	var returnMe = new Array();
	$.getJSON(JSONURL, function(data) {
		callbackFunction(data);
	}); // getJSON
}

function INOUTAPIUpdateUser(person, callbackFunction) {
	$.ajax({
		url:INOUTAPIRoot + '/people/' + person.id,
		method:'PUT',
		dataType:'json',
		data:JSON.stringify(person),
		contentType:"application/json",
		success: function() {
			log("Success updating " + person.id);
			if (callbackFunction != null) {
				callbackFunction();
			}
		},
		error: function(data, status, xhr) {
			log("Failed to update person " + person.id + "");
			log("Error:" + data);
			if (callbackFunction != null) {
				callbackFunction();
			}
		}
	});
}

function INOUTAPIAddUser(person, callbackFunction) {
	$.ajax({
		url:INOUTAPIRoot + '/people/',
		method:'POST',
		dataType:'json',
		data:JSON.stringify(person),
		contentType:"application/json",
		success: function() {
			log("Success adding " + person.displayName);
			if (callbackFunction != null) {
				callbackFunction();
			}
		},
		error: function(data, status, xhr) {
			log("Failed to add person " + person.displayName + "");
			log("Error:" + data);
			if (callbackFunction != null) {
				callbackFunction();
			}
		}
	});
}

function INOUTAPIAddGroup(group, callbackFunction) {
	$.ajax({
		url:INOUTAPIRoot + '/Groups/',
		method:'POST',
		dataType:'json',
		data:JSON.stringify(group),
		contentType:"application/json",
		success: function() {
			log("Success adding " + group.name);
			if (callbackFunction != null) {
				callbackFunction();
			}
		},
		error: function(data, status, xhr) {
			log("Failed to add person " + group.name + "");
			log("Error:" + data);
			if (callbackFunction != null) {
				callbackFunction();
			}
		}
	});
}


function INOUTAPIDeletePerson(person, callbackFunction) {
	$.ajax({
		url:INOUTAPIRoot + '/people/' + person.id,
		method:'DELETE',
		dataType:'json',
		data:JSON.stringify(person),
		contentType:"application/json",
		success: function() {
			log("Success deleting " + person.id);
			if (callbackFunction != null) {
				callbackFunction();
			}
		},
		error: function(data, status, xhr) {
			log("Failed to delete person " + person.id + "");
			log("Error:" + data);
			if (callbackFunction != null) {
				callbackFunction();
			}
		}
	});
}

function INOUTAPIDeleteGroup(group, callbackFunction) {
	$.ajax({
		url:INOUTAPIRoot + '/Groups/' + group.id,
		method:'DELETE',
		dataType:'json',
		data:JSON.stringify(group),
		contentType:"application/json",
		success: function() {
			log("Success deleting " + group.id);
			if (callbackFunction != null) {
				callbackFunction();
			}
		},
		error: function(data, status, xhr) {
			log("Failed to delete group " + group.id + "");
			log("Error:" + data);
			if (callbackFunction != null) {
				callbackFunction();
			}
		}
	});
}

function INOUTAPIUpdateGroup(group, callbackFunction) {
	$.ajax({
		url:INOUTAPIRoot + '/Groups/' + group.id,
		method:'PUT',
		dataType:'json',
		data:JSON.stringify(group),
		contentType:"application/json",
		success: function() {
			log("Success updating " + group.id);
			if (callbackFunction != null) {
				callbackFunction();
			}
		},
		error: function(data, status, xhr) {
			log("Failed to update group " + group.id + "");
			log("Error:" + data);
			if (callbackFunction != null) {
				callbackFunction();
			}
		}
	});
}