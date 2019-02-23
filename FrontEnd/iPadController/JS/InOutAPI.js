var INOUTAPIRoot = "https://inoutapi.lskysd.ca/api/";

function buildPersonHTML(person) {
	var content = "";

	content += "<div class=\"person\">";

	content += "<div class=\"person_name\">";
	content += person.DisplayName;
	content += "</div>";

	content += "<div class=\"person_buttons\">";	
	content += "<div id=\"person_button_" + person.ID + "_in\" class=\"person_button person_button_dim\"><div id=\"person_button_" + person.ID + "_in_contents\" class=\"person_button_contents\" onClick=\"setStatus_In(" + person.ID + ")\">IN</div></div>";
	content += "<div id=\"person_button_" + person.ID + "_out\" class=\"person_button person_button_dim\"><div id=\"person_button_" + person.ID + "_out_contents\" class=\"person_button_contents\" onClick=\"setStatus_Out(" + person.ID + ")\">OUT</div></div>";
	content += "<div id=\"person_button_" + person.ID + "_custom\" class=\"person_button person_button_dim\"><div id=\"person_button_" + person.ID + "_custom_contents\" class=\"person_button_contents_small\">...</div></div>";
	content += "</div>";

	content += "</div>";

	return content;
}

function setPersonUnknown(personID) {
	// Reset status CSS - don't assume it's in a specific state
	$("#person_button_" + personID +"_in").removeClass("person_button_active");
	$("#person_button_" + personID +"_out").removeClass("person_button_active");	
	$("#person_button_" + personID +"_in").removeClass("person_button_dim");
	$("#person_button_" + personID +"_out").removeClass("person_button_dim");

	$("#person_button_" + personID +"_in").addClass("person_button_dim");
	$("#person_button_" + personID +"_out").addClass("person_button_dim");

}

function setPersonIn(personID) {
	// Reset status CSS - don't assume it's in a specific state
	$("#person_button_" + personID +"_in").removeClass("person_button_active");
	$("#person_button_" + personID +"_out").removeClass("person_button_active");	
	$("#person_button_" + personID +"_in").removeClass("person_button_dim");
	$("#person_button_" + personID +"_out").removeClass("person_button_dim");

	$("#person_button_" + personID +"_in").addClass("person_button_active");
	$("#person_button_" + personID +"_out").addClass("person_button_dim");	
}

function setPersonOut(personID) {
	// Reset status CSS - don't assume it's in a specific state
	$("#person_button_" + personID +"_in").removeClass("person_button_active");
	$("#person_button_" + personID +"_out").removeClass("person_button_active");	
	$("#person_button_" + personID +"_in").removeClass("person_button_dim");
	$("#person_button_" + personID +"_out").removeClass("person_button_dim");

	$("#person_button_" + personID +"_in").addClass("person_button_dim");
	$("#person_button_" + personID +"_out").addClass("person_button_active");	

}

function setPersonBusy(personID) {
	$("#person_button_" + personID +"_in").removeClass("person_button_active");
	$("#person_button_" + personID +"_out").removeClass("person_button_active");	
	$("#person_button_" + personID +"_in").removeClass("person_button_dim");
	$("#person_button_" + personID +"_out").removeClass("person_button_dim");


	$("#person_button_" + personID +"_in").addClass("person_button_busy");
	$("#person_button_" + personID +"_out").addClass("person_button_dim");	
}

// Status Types
//  0 Unknown
//  1 IN
//  2 OUT
//  3 BUSY
function updatePersonStatus(personID, status) {
	if (status == null) {
		return;
	}
	
	
	switch(status.StatusType) {
		case 0:
			setPersonUnknown(personID);			
			break;
		case 1:
			setPersonIn(personID);			
			break;
		case 2:
			setPersonOut(personID);			
			break;
		case 3:
			setPersonBusy(personID);			
			break;
		default:
			setPersonUnknown(personID);			
			break;
	}
}

function updateAllPersonStatus() {
	$("#" + intoThisContainer).empty();
	$.getJSON(INOUTAPIRoot + "/People/", function(data) {		
		$.each(data, function(categoryIndex, person) {
			if (person.HasStatus == true) {
				updatePersonStatus(person.ID, person.CurrentStatus);
			}
		}); // each
	}); // getJSON
}

function createUserList(intoThisContainer) {
	$.getJSON(INOUTAPIRoot + "/PeopleWithStatus/", function(data) {	
		$("#" + intoThisContainer).empty();	
		$.each(data, function(categoryIndex, person) {
			
			// Add the user's HTML
			$("#" + intoThisContainer).append(buildPersonHTML(person));

			// Update the user's status display
			if (person.HasStatus == true) {				
				updatePersonStatus(person.ID, person.CurrentStatus);
			}
		}); // each
	}); // getJSON
}

function showLoadingAnimation(divID) {
 	// fas fa-spinner fa-pulse
 	$("#" + divID).addClass("rotate");
}

function removeLoadingAnimation(divID) {
 	$("#" + divID).removeClass("rotate");

}

function showCheckMarkAnimation(divID) {
	removeLoadingAnimation(divID);

	// Get content of the div so we can put it back		
	var oldContents = $("#" + divID).text();

	if (oldContents != "✔") {
		$("#" + divID).addClass("checkmark").text("✔").delay(500).fadeOut(1000, function() {$("#" + divID).text(oldContents).removeClass("checkmark").fadeIn("fast");});	
	}
}

function JSONPostStatus(status, personID, callerDiv) {
	$.ajax({
		url:'https://inoutapi.lskysd.ca/api/status/',
		method:'POST',			
		dataType:'json',
		data:JSON.stringify(status),
		contentType:"application/json",				
		success: function() {
			showCheckMarkAnimation(callerDiv);
			updatePersonStatus(personID, status);
		},
		error: function(data, status, xhr) {
			console.log("Error:");
			console.log(data);
		}
	});
}

// Function to UPDATE the user list, without redrawing it?
function setStatus_In(personID) {
	var callerDiv = "person_button_" + personID + "_in_contents"

	showLoadingAnimation(callerDiv);

	// Build a status object to send
	var today = new Date(); // Quick in and out statuses expire at the end of the day		
	var newStatus = {
		PersonID: personID,
		Expires: today.getFullYear() + "-" + (today.getMonth()+1) + "-" + (today.getDate()+1) + "T00:00:00.000Z", 
		Content: "In",
		StatusType: 1
	}

	// Send the new status to the API
	JSONPostStatus(newStatus, personID, callerDiv);
	
}

function setStatus_Out(personID) {
	var callerDiv = "person_button_" + personID + "_out_contents"
	showLoadingAnimation(callerDiv);	
	

	// Build a status object to send
	var today = new Date(); // Quick in and out statuses expire at the end of the day	
	var newStatus = {
		PersonID: personID,	
		Expires: today.getFullYear() + "-" + (today.getMonth()+1) + "-" + (today.getDate()+1) + "T00:00:00.000Z", 
		Content: "Out",
		StatusType: 2
	}

	// Send the new status to the API
	JSONPostStatus(newStatus, personID, callerDiv);
}