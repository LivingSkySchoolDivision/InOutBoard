var INOUTAPIRoot = "https://inoutapi.lskysd.ca/api/";

// Status Types
//  0 Unknown
//  1 IN
//  2 OUT
//  3 BUSY

function buildPersonHTML(person) {
	var content = "";

	content += "<div class=\"person\">";

	content += "<div class=\"person_name\">";
	content += person.DisplayName;
	content += "</div>";

	content += "<div class=\"person_buttons\">";	
	content += "<div id=\"person_button_" + person.ID + "_in\" class=\"person_button person_button_dim\"><div id=\"person_button_" + person.ID + "_in_contents\" class=\"person_button_contents\" onClick=\"setStatus_In(" + person.ID + ")\">IN</div></div>";
	content += "<div id=\"person_button_" + person.ID + "_out\" class=\"person_button person_button_dim\"><div id=\"person_button_" + person.ID + "_out_contents\" class=\"person_button_contents\" onClick=\"setStatus_Out(" + person.ID + ")\">OUT</div></div>";
	content += "<div id=\"person_button_" + person.ID + "_custom\" class=\"person_button person_button_dim\"><div id=\"person_button_" + person.ID + "_custom_contents_small\" class=\"person_button_contents\">...</div></div>";
	content += "</div>";

	content += "</div>";

	return content;
}

function createUserList(intoThisContainer) {
	$("#" + intoThisContainer).empty();
	$.getJSON(INOUTAPIRoot + "/People/", function(data) {		
		$.each(data, function(categoryIndex, person) {
			$("#" + intoThisContainer).append(buildPersonHTML(person));
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
		success: function(data, status, xhr) {
			showCheckMarkAnimation(callerDiv);
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

	$("#person_button_" + personID +"_in").removeClass("person_button_active");
	$("#person_button_" + personID +"_out").removeClass("person_button_active");	
	$("#person_button_" + personID +"_in").removeClass("person_button_dim");
	$("#person_button_" + personID +"_out").removeClass("person_button_dim");

	$("#person_button_" + personID +"_in").addClass("person_button_active");
	$("#person_button_" + personID +"_out").addClass("person_button_dim");	

	// Build a status object to send
	var today = new Date(); // Quick in and out statuses expire at the end of the day		
	var newStatus = {
		PersonID: personID,
		Expires: today.getFullYear() + "-" + (today.getMonth()+1) + "-" + (today.getDate()+1) + "T00:00:00.000Z", 
		Content: "In",
		StatusType: 1
	}

	JSONPostStatus(newStatus, personID, callerDiv);
	
}

function setStatus_Out(personID) {
	var callerDiv = "person_button_" + personID + "_out_contents"
	showLoadingAnimation(callerDiv);	
	$("#person_button_" + personID +"_in").removeClass("person_button_active");
	$("#person_button_" + personID +"_out").removeClass("person_button_active");	
	$("#person_button_" + personID +"_in").removeClass("person_button_dim");
	$("#person_button_" + personID +"_out").removeClass("person_button_dim");	

	$("#person_button_" + personID +"_in").addClass("person_button_dim");
	$("#person_button_" + personID +"_out").addClass("person_button_active");	
	
	// Build a status object to send
	var expiryDate = new Date(); // Quick in and out statuses expire at the end of the day
	expiryDate.setHours(23,59,59,999);
	var newStatus = {
		PersonID: personID,
		Expires: expiryDate,
		Content: "Out",
		StatusType: 2
	}

	JSONPostStatus(newStatus, personID, callerDiv);
}