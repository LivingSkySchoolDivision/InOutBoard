/* ************************************************************** */
/* * HTML part builders                                         * */
/* ************************************************************** */
var visiblePeopleIDs = new Array();
var filterGroupID = 0;
var pageLoadedTimeStamp = 0;

function buildPersonHTML(person) {

	if (person.id > 0) {
		visiblePeopleIDs.push(person.id);
	}

	var content = "";

	content += "<div class=\"person\" id=\"person_" + person.id + "\">";

	content += "<div class=\"person_status_icon status_icon_unknown\"><img id=\"person_status_icon_" + person.id + "\" class=\"person_status_icon_image\" src=\"../img/circle.svg\"></div>";

	content += "<div class=\"person_name\">";
	content += person.displayName;
	content += "</div>";

	content += "<div class=\"person_status_container\"><div class=\"person_status\" id=\"person_status_" + person.id + "\"><span class=\"status_unknown\">'s whereabouts are unknown.</span></div></div>";

	content += "<div class=\"person_status_text\" id=\"person_status_text_" + person.id + "\"></div>";

	//content += "<div class=\"person_buttons\" id=\"person_buttons_" + person.id + "\">";
	//content += "<div id=\"person_button_" + person.id + "_in\" class=\"person_button person_button_dim noselect\"><div id=\"btnPersonStatusIn_" + person.id +  "\" class=\"person_button_contents noselect\" onClick=\"onclick_btnPersonStatusIn(" + person.id + ")\">IN</div></div>";
	//content += "<div id=\"person_button_" + person.id + "_out\" class=\"person_button person_button_dim noselect\"><div id=\"btnPersonStatusOut_" + person.id + "\" class=\"person_button_contents noselect\" onClick=\"onclick_btnPersonStatusOut(" + person.id + ")\">OUT</div></div>";
	//content += "</div>";
	//content += buildOutOptionsBar(person);

	content += "</div>";

	return content;
}

function buildOutOptionsBar(person) {
	console.log(person);
	var content = "";
	var customStatusText = "";
	if (person.hasStatus == true) {
		if (person.currentStatus.content.length > 0) {
			if ((person.currentStatus.content != "In") && (person.currentStatus.content != "Out")) {
				customStatusText = person.currentStatus.content;
			}
		}
	}
	content += "<div class=\"slide_out_options_bar hidden\" id=\"slide_out_options_bar_" + person.id + "\">";

	content += "<div class=\"slide_out_options_bar_section\">";
	content += "<div class=\"slide_out_options_bar_section_content\">";
	content += "<input type=\"text\" class=\"custom_status_input\" value=\"" + customStatusText + "\"' id=\"txtCustomOutStatus_" + person.id + "\"/>";
	content += "</div>";
	content += "</div>";

	content += "<div class=\"slide_out_options_bar_section\">";
	content += "<div class=\"slide_out_options_bar_section_content\">";
	content += "For the next ";

	content += "<input style=\"vertical-align: middle;\" type=\"tel\" pattern=\"[0-9]*\" class=\"control_days_input\" value=\"1\"' id=\"txtDays_" + person.id + "\"/>";

	content += " days";
	content += "&nbsp;&nbsp;&nbsp;&nbsp;<div style=\"vertical-align: middle;\" class=\"options_bar_button  noselect set_button \"><div id=\"btnSetOutStatus_" + person.id + "\" class=\"options_bar_button_contents set_button_contents  noselect\" onClick=\"onclick_btnSetOutStatus(" + person.id + ");\">SET</div></div>";
	content += "</div>";
	content += "</div>";

	content += "</div>";
	return content;
}

function buildUserList(people) {
	var divID = "user_list";

	// Empty the list
	$("#" + divID).empty();
	people.forEach(function(person) {
		// Add the user's HTML
		$("#" + divID).append(buildPersonHTML(person));
		if (person.hasStatus == true) {
			updatePersonStatus(person.id, person.currentStatus);
		}
	});

	userUpdaterCallback(people);
}

/* ************************************************************** */
/* * Event handling logic                                       * */
/* ************************************************************** */

function onPageLoad() {
	INOUTAPIGetAllUsers(buildUserList);
}

function onInterval_UpdateUserStatuses() {
	INOUTAPIGetAllUsers(userUpdaterCallback);
}


/* ************************************************************** */
/* * Button handling logic                                      * */
/* ************************************************************** */

function onclick_btnPersonStatusIn(personID) {
	setStatus_In(personID);
	hideSlideOutOptionsBar(personID);
}

function onclick_btnPersonStatusOut(personID) {
	toggleSlideOutOptionsBar(personID);
}

function onclick_btnSetOutStatus(personID) {
	setStatus_Out(personID);
	hideSlideOutOptionsBar(personID);
}

/* ************************************************************** */
/* * Options bar Hide / Show Logic                              * */
/* ************************************************************** */
function showSlideOutOptionsBar(personID) {
	var divID = "slide_out_options_bar_" + personID;
	$("#" + divID).slideDown();
	$("#" + divID).removeClass("hidden");

	// Stylize the "out" button to indicate its "open"
	$("#" + "btnPersonStatusOut_" + personID).addClass("options_bar_button_open");
	$("#" + "btnPersonStatusOut_" + personID).html("▼");
	$("#" + "person_buttons_" + personID).addClass("options_bar_buttons_open");

}

function hideSlideOutOptionsBar(personID) {
	var divID = "slide_out_options_bar_" + personID;
	$("#" + divID).slideUp();
	$("#" + divID).addClass("hidden");


	// Stylize the "out" button to indicate its "closed"
	$("#" + "btnPersonStatusOut_" + personID).removeClass("options_bar_button_open");
	$("#" + "person_buttons_" + personID).removeClass("options_bar_buttons_open");
	$("#" + "btnPersonStatusOut_" + personID).html("OUT");

}

function toggleSlideOutOptionsBar(personID) {
	var divID = "slide_out_options_bar_" + personID;

	if ($("#" + divID).hasClass("hidden")) {
		showSlideOutOptionsBar(personID);
	} else {
		hideSlideOutOptionsBar(personID);
	}
}

function hideAllSlideOutOptionsBars() {
}

/* ************************************************************** */
/* * Visual status setting logic                                * */
/* ************************************************************** */

function userUpdaterCallback(people) {
	people.forEach(function(person) {
		if (person.hasStatus == true) {
			console.log("Updating status for " + person.displayName);
			updatePersonStatus(person.id, person.currentStatus);
		}
	});
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

	var personDiv_status_word = "#person_status_" + personID;
	var personDiv_status_text = "#person_status_text_" + personID;
	var personDiv_status_icon = "#person_status_icon_" + personID;

	switch(status.statusType) {
		case 1:
			// In
			$(personDiv_status_word).html(" is <b class=\"status_word_in\">IN</b>");
			$(personDiv_status_text).html("");			
			$(personDiv_status_icon).attr("src", "../img/circle-tick.svg");
			break;
		case 2:
			// Out
			$(personDiv_status_word).html(" is <b class=\"status_word_out\">OUT</b>");
			if (status.content.length > 0) {
				$(personDiv_status_text).html(" - " + status.content);
			}	
			$(personDiv_status_icon).attr("src", "../img/circle-delete.svg");		
			break;
		default:	
			$(personDiv_status_icon).attr("src", "../img/circle.svg");
			$(personDiv_status_text).html("");						
			break;
	}
}

function setStatus_In(personID) {
	var callerDiv = "person_button_" + personID + "_in_contents"

	// Build a status object to send
	var today = new Date(); // Quick in and out statuses expire at the end of the day
	today.setHours(24,(today.getTimezoneOffset() * -1),0,0);
	var todayString = today.toJSON()
	var newStatus = {
		personID: personID,
		expires: todayString,
		content: "",
		statusType: 1
	}

	// Send the new status to the API
	INOUTAPIPostStatus(personID, newStatus, updatePersonStatus);
}

function setStatus_Out(personID) {
	var callerDiv = "person_button_" + personID + "_out_contents"
	var custom_status = $("#txtCustomOutStatus_" + personID).val();
	var custom_days = $("#txtDays_" + personID).val();

	var days = parseInt(custom_days);
	if (days < 1) {
		days = 1;
	}

	// Build a status object to send
	var today = new Date(); // Quick in and out statuses expire at the end of the day
	today.setHours((24 * days),(today.getTimezoneOffset() * -1),0,0);
	var todayString = today.toJSON()
	var newStatus = {
		personID: personID,
		expires: todayString,
		content: custom_status,
		statusType: 2
	}

	// Send the new status to the API
	INOUTAPIPostStatus(personID, newStatus, updatePersonStatus);
}

/* ************************************************************** */
/* * Effects and animations                                     * */
/* ************************************************************** */

function showCheckMarkAnimation(divID) {
	removeLoadingAnimation(divID);

	// Get content of the div so we can put it back
	var oldContents = $("#" + divID).text();

	if (oldContents != "✔") {
		$("#" + divID).addClass("checkmark").text("✔").delay(500).fadeOut(1000, function() {$("#" + divID).text(oldContents).removeClass("checkmark").fadeIn("fast");});
	}
}