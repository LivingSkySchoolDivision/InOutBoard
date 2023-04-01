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

	content += "<div class=\"person\">";

	content += "<div class=\"person_name\">";
	content += person.displayName;
	content += "</div>";

	content += "<div class=\"person_status_container\"><div class=\"person_status status_unknown\" id=\"person_status_" + person.id + "\">'s whereabouts are unknown.</div></div>";

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

	// Hide the loading animation
	$("#loading").fadeOut();

	// Show the title bar
	$("#title_bar").show();
}

function buildFilterButton(group) {
	var content = "";

	content += "<div id=\"btnChangeFilter_" + group.id + "\" class=\"filter_option\" onclick=\"onClick_btnChangeFilter(" + group.id + ");\">" + group.name + "</div>";

	return content;
}

function buildGroupList(groups) {
	console.log("Building group list");

	var divID = "mnuFilterSelect";

	// Empty the list
	$("#" + divID).empty();

	// Add "All people" group
	$("#" + divID).append(buildFilterButton({ "id":0, "name":"Everyone (no filter)" }));

	// Add all other groups
	groups.forEach(function(group) {

		// Add the user's HTML
		$("#" + divID).append(buildFilterButton(group));
	});

	// Hide the loading animation
	$("#loading").fadeOut();

	// Show the title bar
	$("#title_bar").show();
}

/* ************************************************************** */
/* * Event handling logic                                       * */
/* ************************************************************** */

function onPageLoad() {
	// Load the list of all groups
	INOUTAPIGetAllGroups(buildGroupList);

 	// Load all group members
	INOUTAPIGetGroupMembers(filterGroupID,buildUserList);
}

function onInterval_UpdateUserStatuses() {
	INOUTAPIGetGroupMembers(filterGroupID,userUpdaterCallback);
}

function userUpdaterCallback(people) {
	people.forEach(function(person) {
		if (person.hasStatus == true) {
			updatePersonStatus(person.id, person.currentStatus);
		}
	});
}

function filterNameCallback(group) {
	var groupName = "Everyone (no filter)";

	if (group != null) {
		groupName = group.name;
	}

	update_filter_name(group.name);
}

function filterUpdateCallback(group) {
	var groupID = 0;
	var groupName = "Everyone (no filter)";

	if (group != null) {
		groupID = group.id;
		groupName = group.name;
	}

	// Set the filter
	setFilter(groupID);

	// Update the filter name
	update_filter_name(groupName);

	// Refresh the list of users
	INOUTAPIGetGroupMembers(groupID ,buildUserList);

	// Hide the filter menu
	hideFilterSelect();
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

function onclick_TitleBar() {
 	toggleFilterSelect();
}

function onClick_btnChangeFilter(groupID) {
	INOUTAPIGetGroup(groupID, filterUpdateCallback);
}

/* ************************************************************** */
/* * Title bar / Filter bar logic                               * */
/* ************************************************************** */

function showFilterSelect() {
	var divID = "mnuFilterSelect";
	$("#" + divID).slideDown();
	$("#" + divID).removeClass("hidden");
}

function hideFilterSelect() {
	var divID = "mnuFilterSelect";
	$("#" + divID).slideUp();
	$("#" + divID).addClass("hidden");
}

function toggleFilterSelect() {
	var divID = "mnuFilterSelect";

	if ($("#" + divID).hasClass("hidden")) {
		showFilterSelect();
	} else {
		hideFilterSelect();
	}
}

function update_filter_name(groupName) {
	$("#lblFilterBy").text(groupName);
}

function setFilter(groupID) {
	console.log("setting filter to " + groupID);

	// Set the filter variable here (for as long as this page doesn't refresh)
	filterGroupID = groupID;

	// Save this to a cookie if we can (so we can pick it up if the page does refresh)
	Cookies.set("lssd_inout_filter_groupid", groupID, { expires: 3650 });

	// Update filter name to the new group name
	update_filter_name(groupID);
}

function getFilterFromCookies() {
  	return Cookies.get("lssd_inout_filter_groupid");
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
			updatePersonStatus(person.id, person.currentStatus);
		}
	});
}

function setPersonCustomStatus(personID, status) {
	$("#txtCustomOutStatus_" + personID).val(status);
}

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

	switch(status.statusType) {
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

	// Update their custom text
	var customStatusText = "";
	if (status.content.length > 0) {
		setPersonCustomStatus(personID, status.content);
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

/* ************************************************************** */
/* * Page reload watchdog                                       * */
/* ************************************************************** */

function initializePageWatchdog() {
	var date = new Date();
	pageLoadedTimeStamp = date.getTime();
	$("#last_refresh_time").html(date.toTimeString());
	$("#last_refresh_date").html(date.toDateString());
}

function checkPageWatchdog() {
	var date = new Date();
	var currentTimestamp = date.getTime();

	// Show warning at 6 minutes (1 minute after the page should have reloaded)
	if ((currentTimestamp - pageLoadedTimeStamp) > 360000) {
		$("#watchdog_reload_warning_message").fadeIn();
	}
}