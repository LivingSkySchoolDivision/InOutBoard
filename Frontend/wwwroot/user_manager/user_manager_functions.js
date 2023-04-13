
var all_groups = [];
var displayed_person = null;
var displayed_group = null;

/* ************************************************************** */
/* * HTML part builders                                         * */
/* ************************************************************** */
function buildPersonHTML(person)
{

	if (person.id > 0) {
		visiblePeopleIDs.push(person.id);
	}

	var content = "";

	content += "<div class=\"person\">";

	content += "<div class=\"person_name\">";
	content += person.displayName;
	content += "</div>";

	content += "<div class=\"person_status_container\"><div class=\"person_status status_unknown\" id=\"person_status_" + person.id + "\">'s whereabouts are unknown.</div></div>";

	content += "</div>";

	return content;
}

function refresh_all_lists()
{
	console.log("Refreshing all lists!");
	refresh_people_list();
	refresh_group_list();
}

/* ************************************************************** */
/* * API loading - groups                                       * */
/* ************************************************************** */


function refresh_group_list()
{
	console.log("Refreshing group list 1/2");
	INOUTAPIGetAllGroups(rebuild_group_lists);
}

function rebuild_group_lists(groups)
{
	console.log("Refreshing group list 2/2");
	var div_groups = "group_list";

	$("#" + div_groups).empty();

	groups.forEach(group => {
		$("#" + div_groups).append(build_group_record_active(group));
	});

}

function build_group_record_active(group)
{
	var content = "";

	content += "<div class='person'>";
	content += "<div class='person_name' onclick=\"show_group_modal(" + group.id + ");\">" + group.name +"</div>";
	content += "</div>";

	return content;
}

function show_group_modal(groupid)
{
	hide_person_modal();

	// Load this user's data
	INOUTAPIGetGroup(groupid, update_group_modal_data);
	INOUTAPIGetGroupMembers(groupid, update_group_modal_member_list);
	$("#group_modal").fadeIn();
}

function update_group_modal_data(group)
{
	displayed_group = group;

	$("#group_modal_group_name").val(group.name)
	$("#group_modal_groupid").val(group.id)
}

function update_group_modal_member_list(groupmembers)
{
	$("#group_modal_member_list").empty();

	groupmembers.forEach(person => {
		$("#group_modal_member_list").append(get_group_member_html(person));
	});

}

function save_group_modal()
{
	if (displayed_group != null)
	{
		displayed_group.name = $("#group_modal_group_name").val();
		console.log(displayed_group);
		INOUTAPIUpdateGroup(displayed_group, refresh_all_lists);
		hide_group_modal();
	}
}

function hide_group_modal()
{
	//displayed_person = null;
	$("#group_modal").fadeOut();
	refresh_all_lists();
}

function get_group_member_html(person)
{
	var content = "";
	if (person != null)
	{
		content += "<div class='group_modal_member' id='group_modal_member_" + person.id + "'>" + person.displayName + " <div class='user_modal_group_remove_link' onclick=\"remove_group_member(" + person.id + ");\">(remove)</div></div>";
	}
	return content;
}

function remove_group_member(personid)
{
	// Get the currently viewed group
	if (displayed_group != null)
	{
		var person = INOUTAPIGetUser(personid, remove_group_member_stage_2);
	}
}

function remove_group_member_stage_2(person)
{
	if ((displayed_group != null) && (person != null))
	{
		var new_groups_array = new Array();
		displayed_person = person;
		console.log("Removing " + displayed_person.displayName + " from group " + displayed_group.id);

		if(Array.isArray(displayed_person.groupsIDs)) {
			displayed_person.groupsIDs.forEach(groupid => {
				if (groupid != displayed_group.id) {
					new_groups_array.push(groupid);
				}
			});
		}

		console.log(new_groups_array);

		displayed_person.groupsIDs = new_groups_array;

		console.log(displayed_person.groupsIDs);

		// Remove the member from the modal list
		$("#group_modal_member_" + person.id).fadeOut();

		// Send the update to the API
		INOUTAPIUpdateUser(displayed_person);
	}
}

function add_new_group()
{
	var new_group = {};
	new_group.name = $("#new_group_name").val();
	$("#new_group_name").val("");
	INOUTAPIAddGroup(new_group, refresh_all_lists);
}

function delete_group()
{
	if (displayed_group != null)
	{
		INOUTAPIDeleteGroup(displayed_group, refresh_all_lists);
	}
	hide_group_modal();
}

/* ************************************************************** */
/* * API loading - People                                       * */
/* ************************************************************** */


function refresh_people_list()
{
    INOUTAPIGetAllUsers(rebuild_user_lists);
}

function rebuild_user_lists(people)
{
	var div_users = "user_list";

	$("#" + div_users).empty();

	people.forEach(person => {
		$("#" + div_users).append(build_person_record_active(person));
	});

}

function build_person_record_active(person)
{
	var content = "";

	content += "<div class='person'>";
	content += "<div class='person_name' onclick=\"show_person_modal(" + person.id + ");\">" + person.displayName +"</div>";
	content += "</div>";

	return content;
}


function show_person_modal(personid)
{
	hide_group_modal();

	// Load list of groups
	all_groups = INOUTAPIGetAllGroups(refresh_user_group_list);


	// Load this user's data
	var person = INOUTAPIGetUser(personid, update_person_modal_data);
	$("#person_modal").fadeIn();
	refresh_all_lists();
}

function update_person_modal_data(person)
{
	displayed_person = person;
	$("#person_modal_first_name").val(person.firstName)
	$("#person_modal_last_name").val(person.lastName)
	$("#person_modal_personid").val(person.id)

	$("#person_modal_group_list").empty();
	// Load their groups
	if (Array.isArray(person.groupsIDs))
	{
		person.groupsIDs.forEach(groupid => {
			var thisgroup = get_group(groupid);
			$("#person_modal_group_list").append(get_group_html(thisgroup));
		});
	}

	// Load the existing groups into the dropdown
	$("#person_modal_group_select").empty();
	all_groups.forEach(group => {
		$("#person_modal_group_select").append($('<option>', { value: group.id, text: group.name }));
	});
}

function modal_group_add_button()
{
	var new_group_id = $("#person_modal_group_select").find(":selected").val();
	add_assigned_group(new_group_id);
}

function remove_assigned_group(groupid)
{
	var new_groups_array = new Array();

	if(Array.isArray(displayed_person.groupsIDs)) {
		displayed_person.groupsIDs.forEach(group => {
			if (group != groupid) {
				new_groups_array.push(group);
			}
		});
	}

	displayed_person.groupsIDs = new_groups_array;
	update_person_modal_data(displayed_person);
	refresh_all_lists();
}

function add_assigned_group(groupid)
{
	var exists = false;

	if(Array.isArray(displayed_person.groupsIDs)) {
		displayed_person.groupsIDs.forEach(group => {
			if (group == groupid) {
				exists = true;
			}
		});
	}

	if (exists == false)
	{
		displayed_person.groupsIDs.push(groupid);
	}

	update_person_modal_data(displayed_person);
	refresh_all_lists();
}

function get_group_html(group)
{
	var content = "";
	if (group != null)
	{
		content += "<div class='user_modal_group'>" + group.name + " <div class='user_modal_group_remove_link' onclick=\"remove_assigned_group(" + group.id + ");\">(remove)</div></div>";
	}
	return content;
}

function save_person_modal()
{
	if (displayed_person != null)
	{
		// Load in all the stuff from the person modal
		displayed_person.firstName = $("#person_modal_first_name").val();
		displayed_person.lastName = $("#person_modal_last_name").val();

		// Send the update
		INOUTAPIUpdateUser(displayed_person, refresh_all_lists);
	}
	hide_person_modal();
}

function hide_person_modal()
{
	displayed_person = null;
	$("#person_modal").fadeOut();
	refresh_all_lists();
}

function refresh_user_group_list(groups) {
	all_groups = groups;
}

function get_group(groupid)
{
	var found_group = null;
	if (Array.isArray(all_groups))
	{
		all_groups.forEach(group => {
			if (group.id == groupid) {
				found_group = group;
			}
		});
	}
	return found_group;
}

function add_new_person()
{
	var new_person = { isEnabled: true, groupsIDs: [], hasStatus: false };

	new_person.firstName = $("#new_person_first_name").val();
	new_person.lastName = $("#new_person_last_name").val()
	new_person.displayName = new_person.firstName + " " + new_person.lastName;
	INOUTAPIAddUser(new_person, refresh_all_lists);
	new_person.firstName = $("#new_person_first_name").val("");
	new_person.lastName = $("#new_person_last_name").val("");
}

function delete_person()
{
	if (displayed_person != null)
	{
		INOUTAPIDeletePerson(displayed_person, refresh_all_lists);
	}
	refresh_all_lists();
	hide_person_modal();
}