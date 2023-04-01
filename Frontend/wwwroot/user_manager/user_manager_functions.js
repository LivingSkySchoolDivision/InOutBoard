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

	//content += "<div class=\"person_buttons\" id=\"person_buttons_" + person.id + "\">";
	//content += "<div id=\"person_button_" + person.id + "_in\" class=\"person_button person_button_dim noselect\"><div id=\"btnPersonStatusIn_" + person.id +  "\" class=\"person_button_contents noselect\" onClick=\"onclick_btnPersonStatusIn(" + person.id + ")\">IN</div></div>";
	//content += "<div id=\"person_button_" + person.id + "_out\" class=\"person_button person_button_dim noselect\"><div id=\"btnPersonStatusOut_" + person.id + "\" class=\"person_button_contents noselect\" onClick=\"onclick_btnPersonStatusOut(" + person.id + ")\">OUT</div></div>";
	//content += "</div>";
	//content += buildOutOptionsBar(person);

	content += "</div>";

	return content;
}

/* ************************************************************** */
/* * API loading                                                * */
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

var all_groups = [];
var displayed_person = null;

function show_person_modal(personid)
{
	// Load list of groups
	all_groups = INOUTAPIGetAllGroups(refresh_group_list);


	// Load this user's data
	var person = INOUTAPIGetUser(personid, update_person_modal_data);
	$("#person_modal").fadeIn();
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
		INOUTAPIUpdateUser(displayed_person);

		hide_person_modal();
	}
}

function hide_person_modal()
{
	displayed_person = null;
	$("#person_modal").fadeOut();
}

function refresh_group_list(groups) {
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