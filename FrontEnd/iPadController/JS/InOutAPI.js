var INOUTAPIRoot = "https://inoutapi.lskysd.ca/api/";

/* ************************************************************** */
/* * HTML part builders                                         * */
/* ************************************************************** */

function buildPersonHTML(person) {
	var content = "";

	content += "<div class=\"person\">";

	content += "<div class=\"person_name\">";
	content += person.DisplayName;
	content += "</div>";

	content += "<div class=\"person_buttons\">";	
	content += "<div id=\"person_button_" + person.ID + "_in\" class=\"person_button person_button_dim\"><div id=\"btnPersonStatusIn_" + person.ID +  "\" class=\"person_button_contents\" onClick=\"onclick_btnPersonStatusIn(" + person.ID + ")\">IN</div></div>";
	content += "<div id=\"person_button_" + person.ID + "_out\" class=\"person_button person_button_dim\"><div id=\"btnPersonStatusOut_" + person.ID + "\" class=\"person_button_contents\" onClick=\"onclick_btnPersonStatusOut(" + person.ID + ")\">OUT</div></div>";
	content += "</div>";

	content += buildOutOptionsBar(person);

	content += "</div>";

	return content;
}

function buildOutOptionsBar(person) {
	var content = "";
	var customStatusText = "";
	if (person.HasStatus == true) {
		if (person.CurrentStatus.Content.length > 0) {
			if ((person.CurrentStatus.Content != "In") && (person.CurrentStatus.Content != "Out")) {
				customStatusText = person.CurrentStatus.Content;
			}
		}
	}
	content += "<div class=\"slide_out_options_bar hidden\" id=\"slide_out_options_bar_" + person.ID + "\">";
	
	content += "<div class=\"slide_out_options_bar_section\">";
	content += "<div class=\"slide_out_options_bar_section_content\">";
	content += "<input type=\"text\" class=\"custom_status_input\" value=\"" + customStatusText + "\"' id=\"txtCustomOutStatus_" + person.ID + "\"/>";	
	content += "</div>";
	content += "</div>";

	content += "<div class=\"slide_out_options_bar_section\">";
	content += "<div class=\"slide_out_options_bar_section_content\">";
	content += "For the next ";
	
	content += "<input style=\"vertical-align: middle;\" type=\"tel\" pattern=\"[0-9]*\" class=\"control_days_input\" value=\"1\"' id=\"txtDays_" + person.ID + "\"/>";

	content += " days";
	content += "&nbsp;&nbsp;&nbsp;&nbsp;<div style=\"vertical-align: middle;\" class=\"options_bar_button \"><div id=\"btnSetOutStatus_" + person.ID + "\" class=\"options_bar_button_contents set_button\" onClick=\"onclick_btnSetOutStatus(" + person.ID + ");\">SET</div></div>";
	content += "</div>";
	content += "</div>";

	content += "</div>";	
	return content;
}


/* ************************************************************** */
/* * Button handling logic                                      * */
/* ************************************************************** */

function onclick_btnPersonStatusIn(personID) {
	setStatus_In(personID);
}

function onclick_btnPersonStatusOut(personID) {
	toggleSlideOutOptionsBar(personID);
}

function onclick_btnSetOutStatus(personID) {
	setStatus_Out(personID); // gather the data here, and make the function only do the AJAX stuff
	hideSlideOutOptionsBar(personID);

	//refreshStatus(personID);
}

/* ************************************************************** */
/* * Options bar Hide / Show Logic                              * */
/* ************************************************************** */

function showSlideOutOptionsBar(personID) {
	var divID = "slide_out_options_bar_" + personID;
	$("#" + divID).slideDown();
	$("#" + divID).removeClass("hidden");

}

function hideSlideOutOptionsBar(personID) {
	var divID = "slide_out_options_bar_" + personID;
	$("#" + divID).slideUp();
	$("#" + divID).addClass("hidden");

}


function toggleSlideOutOptionsBar(personID) {
	var divID = "slide_out_options_bar_" + personID;

	if ($("#" + divID).hasClass("hidden")) {
		showSlideOutOptionsBar(personID);
	} else {
		hideSlideOutOptionsBar(personID);
	}
}


/* ************************************************************** */
/* * Options bar logic                                          * */
/* ************************************************************** */

function addDay(personID) {
	var divID = "txtDays_" + personID;
	var numDays = $("#" + divID).val();
	numDays++;
	$("#" + divID).val(numDays);
		
}

function removeDay(personID) {
	var divID = "txtDays_" + personID;
	var numDays = $("#" + divID).val();
	numDays--;
	if (numDays < 1) {
		numDays = 1;
	}
	$("#" + divID).val(numDays);
}

/* ************************************************************** */
/* * Visual status setting logic                                * */
/* ************************************************************** */

function setPersonUnknown(personID) {
	// Reset status CSS - don't assume it's in a specific state
	$("#person_button_" + personID +"_in").removeClass("person_button_active");
	$("#person_button_" + personID +"_out").removeClass("person_button_active");	
	$("#person_button_" + personID +"_in").removeClass("person_button_dim");
	$("#person_button_" + personID +"_out").removeClass("person_button_dim");	
	$("#person_button_" + personID +"_in").removeClass("person_button_busy");
	$("#person_button_" + personID +"_out").removeClass("person_button_busy");	
	$("#person_button_" + personID +"_in").addClass("person_button_dim");
	$("#person_button_" + personID +"_out").addClass("person_button_dim");

}

function setPersonIn(personID) {
	// Reset status CSS - don't assume it's in a specific state
	$("#person_button_" + personID +"_in").removeClass("person_button_active");
	$("#person_button_" + personID +"_out").removeClass("person_button_active");	
	$("#person_button_" + personID +"_in").removeClass("person_button_dim");
	$("#person_button_" + personID +"_out").removeClass("person_button_dim");	
	$("#person_button_" + personID +"_in").removeClass("person_button_busy");
	$("#person_button_" + personID +"_out").removeClass("person_button_busy");

	$("#person_button_" + personID +"_in").addClass("person_button_active");
	$("#person_button_" + personID +"_out").addClass("person_button_dim");	
}

function setPersonOut(personID) {
	// Reset status CSS - don't assume it's in a specific state
	$("#person_button_" + personID +"_in").removeClass("person_button_active");
	$("#person_button_" + personID +"_out").removeClass("person_button_active");	
	$("#person_button_" + personID +"_in").removeClass("person_button_dim");
	$("#person_button_" + personID +"_out").removeClass("person_button_dim");	
	$("#person_button_" + personID +"_in").removeClass("person_button_busy");
	$("#person_button_" + personID +"_out").removeClass("person_button_busy");

	$("#person_button_" + personID +"_in").addClass("person_button_dim");
	$("#person_button_" + personID +"_out").addClass("person_button_active");	
}

function setPersonBusy(personID) {
	$("#person_button_" + personID +"_in").removeClass("person_button_active");
	$("#person_button_" + personID +"_out").removeClass("person_button_active");	
	$("#person_button_" + personID +"_in").removeClass("person_button_dim");
	$("#person_button_" + personID +"_out").removeClass("person_button_dim");	
	$("#person_button_" + personID +"_in").removeClass("person_button_busy");
	$("#person_button_" + personID +"_out").removeClass("person_button_busy");


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

	//hideOutControlBar(personID);
	//hideTimeControlBar(personID);
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


/* ************************************************************** */
/* * AJAX / JSON Logic                                          * */
/* ************************************************************** */

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

function setStatus_In(personID) {
	var callerDiv = "person_button_" + personID + "_in_contents"

	showLoadingAnimation(callerDiv);

	// Build a status object to send
	var today = new Date(); // Quick in and out statuses expire at the end of the day	
	today.setHours(24,(today.getTimezoneOffset() * -1),0,0);
	var todayString = today.toJSON()
	console.log(todayString);	
	var newStatus = {
		PersonID: personID,
		Expires: todayString, 
		Content: "",
		StatusType: 1
	}

	// Send the new status to the API
	JSONPostStatus(newStatus, personID, callerDiv);
	
}

function setStatus_Out(personID) {
	var callerDiv = "person_button_" + personID + "_out_contents"
	var custom_status = $("#txtCustomOutStatus_" + personID).val();
	var custom_days = $("#txtDays_" + personID).val();

	var days = parseInt(custom_days);
	if (days < 1) {
		days = 1;
	}

	showLoadingAnimation(callerDiv);

	// Build a status object to send
	var today = new Date(); // Quick in and out statuses expire at the end of the day	
	today.setHours((24 * days),(today.getTimezoneOffset() * -1),0,0);
	var todayString = today.toJSON()
	console.log(todayString);	
	var newStatus = {
		PersonID: personID,
		Expires: todayString, 
		Content: custom_status,
		StatusType: 2
	}

	// Send the new status to the API
	JSONPostStatus(newStatus, personID, callerDiv);
}

function setStatus_Busy(personID) {
	var callerDiv = "options_bar_button" + personID + "_busy_contents"

	showLoadingAnimation(callerDiv);

	// Build a status object to send
	var today = new Date(); // Quick in and out statuses expire at the end of the day		
	var newStatus = {
		PersonID: personID,
		Expires: today.getFullYear() + "-" + (today.getMonth()+1) + "-" + (today.getDate()+1) + "T00:00:00.000Z", 
		Content: "",
		StatusType: 3
	}

	// Send the new status to the API
	JSONPostStatus(newStatus, personID, callerDiv);	
}

