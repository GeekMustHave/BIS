<%@ Page Title="Requirements" Language="C#" MasterPageFile="~/BISMaster.master" AutoEventWireup="true"
    CodeFile="Requirements.aspx.cs" Inherits="Requirements" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControlToolkit" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register TagPrefix="CustomButtonDropDown" TagName="ButtonDropDown" Src="~/Plugins/UserControls/buttonDropDown.ascx" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <link href="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0/css/select2.min.css?vv=11"
        rel="stylesheet" />
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0/js/select2.min.js"></script>
    <link href="Styles/PrntScrnPasteImage.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/paste.js"></script>
    <script type="text/javascript" src="Plugins/sticky/jquery.sticky.js"></script>
    <style type="text/css">
        /* increase z-index to higher to show all the menus while loading on a popup.*/
        .cke_dialog {
            z-index: 1000001 !important;
        }

        .cke_bottom {
            display: none !important;
        }

        .cke_panel {
            z-index: 1000001 !important;
        }

        /* change z index to show the select2 dropdown on modal popup. Labels Dropdown*/
        .select2-dropdown {
            z-index: 1000005 !important;
            border: 1px solid #d3d3d3 !important;
        }
        /* For not to highlight the labels textbox - removing border*/
        .select2-container--default .select2-selection--multiple {
            border: 1px solid #d3d3d3 !important;
        }
        /* css for file drop zone */
        .image-drop-zone {
            border: 2px dashed #d3d3d3;
            border-radius: 0;
            padding: 7px;
            position: relative;
            transition: background-color 0.01s linear 0.01s;
            min-height: 23px;
        }

        .browselinkcss {
            cursor: pointer;
            color: #034af3;
        }

            .browselinkcss:hover {
                text-decoration: underline;
            }

        .image-drop-zone__drop-icon::before {
            background-image: url("data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9Im5vIj8+Cjxzdmcgd2lkdGg9IjI1cHgiIGhlaWdodD0iMjFweCIgdmlld0JveD0iMCAwIDI1IDIxIiB2ZXJzaW9uPSIxLjEiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgeG1sbnM6eGxpbms9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkveGxpbmsiIHhtbG5zOnNrZXRjaD0iaHR0cDovL3d3dy5ib2hlbWlhbmNvZGluZy5jb20vc2tldGNoL25zIj4KICAgIDxnIGlkPSJQYWdlLTEiIHN0cm9rZT0ibm9uZSIgc3Ryb2tlLXdpZHRoPSIxIiBmaWxsPSJub25lIiBmaWxsLXJ1bGU9ImV2ZW5vZGQiIHNrZXRjaDp0eXBlPSJNU1BhZ2UiPgogICAgICAgIDxnIGlkPSIwNV9Ecm9wem9uZS0oQ0EpLS0tdXBsb2FkZWQiIHNrZXRjaDp0eXBlPSJNU0FydGJvYXJkR3JvdXAiIHRyYW5zZm9ybT0idHJhbnNsYXRlKC0zMTMuMDAwMDAwLCAtNjQ1LjAwMDAwMCkiIGZpbGw9IiM3MDcwNzAiPgogICAgICAgICAgICA8ZyBpZD0iSXNzdWUtQXR0YWNobWVudHMiIHNrZXRjaDp0eXBlPSJNU0xheWVyR3JvdXAiIHRyYW5zZm9ybT0idHJhbnNsYXRlKDIwLjAwMDAwMCwgNjAyLjAwMDAwMCkiPgogICAgICAgICAgICAgICAgPGcgaWQ9IkRyYWctJmFtcDstZHJvcC1lbXB0eS0yIiB0cmFuc2Zvcm09InRyYW5zbGF0ZSgyOTMuMDAwMDAwLCA0My4wMDAwMDApIiBza2V0Y2g6dHlwZT0iTVNTaGFwZUdyb3VwIj4KICAgICAgICAgICAgICAgICAgICA8ZyBpZD0iY2xvdWQtdXBsb2FkLTIiPgogICAgICAgICAgICAgICAgICAgICAgICA8cGF0aCBkPSJNMTMuMTI0Nzk3NywxOS42NDc5NDEgTDEzLjEyNDc5NzcsMTEuMDU2MzA2IEwxNS4xNzM3OTIyLDEzLjEzMDYxMDQgQzE1LjQ1NzI5NTcsMTMuNDE2NjYzNiAxNS45MTQzNzI3LDEzLjQxNjY2MzYgMTYuMTk4NzAyNywxMy4xMzA2MTA0IEMxNi40ODEzNzk2LDEyLjg0MzcyMDcgMTYuNDgxMzc5NiwxMi4zODAzNDc5IDE2LjE5ODcwMjcsMTIuMDk0Mjk0NiBMMTIuOTEzMjAzMyw4Ljc2OTU1MjY3IEMxMi44Nzg0ODg2LDguNzM1MjU5NzMgMTIuODQwNDY3Nyw4LjcwNTE0ODg2IDEyLjgwMTYyMDMsOC42NzgzODM2NCBDMTIuNzg1MDg5NSw4LjY2NzUxMDI3IDEyLjc2NzczMjEsOC42NjA4MTg5NyAxMi43NTEyMDEzLDguNjUwNzgyMDEgQzEyLjcyNzIzMTYsOC42MzY1NjI5OSAxMi43MDI0MzU0LDguNjIyMzQzOTcgMTIuNjc2ODEyNyw4LjYxMTQ3MDYgQzEyLjY1NTMyMjYsOC42MDIyNzAwNSAxMi42MzMwMDYsOC41OTcyNTE1OCAxMi42MTE1MTU5LDguNTkxMzk2NjggQzEyLjU4ODM3MjgsOC41ODM4Njg5NyAxMi41NjUyMjk3LDguNTc0NjY4NDIgMTIuNTQxMjYsOC41NzA0ODYzNiBDMTIuNTA0MDY1Nyw4LjU2MjEyMjIzIDEyLjQ2Njg3MTMsOC41NjA0NDk0IDEyLjQyOTY3Nyw4LjU1ODc3NjU3IEMxMi40MTg5MzIsOC41NTg3NzY1NyAxMi40MTA2NjY1LDguNTU1NDMwOTIgMTIuMzk5MDk1LDguNTU1NDMwOTIgQzEyLjM4NzUyMzQsOC41NTU0MzA5MiAxMi4zNzg0MzE1LDguNTU4Nzc2NTcgMTIuMzY2ODU5OSw4LjU1ODc3NjU3IEMxMi4zMzA0OTIxLDguNTYwNDQ5NCAxMi4yOTQxMjQzLDguNTYyOTU4NjQgMTIuMjU4NTgzMSw4LjU3MDQ4NjM2IEMxMi4yMzI5NjAzLDguNTc1NTA0ODQgMTIuMjA4OTkwNiw4LjU4NTU0MTc5IDEyLjE4NDE5NDQsOC41OTMwNjk1MSBDMTIuMTYzNTMwOSw4LjU5OTc2MDgyIDEyLjE0MzY5MzksOC42MDMxMDY0NyAxMi4xMjMwMzA0LDguNjExNDcwNiBDMTIuMDk0OTI4LDguNjIzMTgwMzggMTIuMDY5MzA1Miw4LjYzODIzNTgyIDEyLjA0Mjg1NTksOC42NTQxMjc2NiBDMTIuMDI3OTc4Miw4LjY2MjQ5MTc5IDEyLjAxMzEwMDUsOC42NjgzNDY2OSAxMS45OTgyMjI3LDguNjc3NTQ3MjMgQzExLjk1NzcyMjIsOC43MDUxNDg4NiAxMS45MjA1Mjc5LDguNzM2MDk2MTQgMTEuODg2NjM5Nyw4Ljc3MDM4OTA4IEw4LjYwMTk2NjksMTIuMDk0Mjk0NiBDOC40NjA2Mjg0NCwxMi4yMzczMjEyIDguMzg5NTQ1OTQsMTIuNDI1NTE0MiA4LjM4OTU0NTk0LDEyLjYxMjAzNDMgQzguMzg5NTQ1OTQsMTIuODAwMjI3MiA4LjQ2MDYyODQ0LDEyLjk4NzU4MzcgOC42MDE5NjY5LDEzLjEzMDYxMDQgQzguODg0NjQzODMsMTMuNDE2NjYzNiA5LjM0MjU0NzM5LDEzLjQxNjY2MzYgOS42MjYwNTA4NiwxMy4xMzA2MTA0IEwxMS42NzUwNDUzLDExLjA1NjMwNiBMMTEuNjc1MDQ1MywxOS42NDg3Nzc0IEMxMS42NzUwNDUzLDIwLjA1MzYwMTMgMTEuOTk5MDQ5MywyMC4zODIzMTE2IDEyLjM5ODI2ODQsMjAuMzgyMzExNiBDMTIuODAwNzkzOCwyMC4zODE0NzUyIDEzLjEyNDc5NzcsMjAuMDUzNjAxMyAxMy4xMjQ3OTc3LDE5LjY0Nzk0MSBMMTMuMTI0Nzk3NywxOS42NDc5NDEgWiIgaWQ9IlNoYXBlIj48L3BhdGg+CiAgICAgICAgICAgICAgICAgICAgICAgIDxwYXRoIGQ9Ik0yMC43MDMzNDk2LDYuMjg0NTY5NDUgQzIwLjQ3OTM1NzEsMi45MTQ2NjEyMiAxNy42OTgwNDc1LDAuMjQzMTU3ODk1IDE0LjMxMjUzNjgsMC4yNDMxNTc4OTUgQzEyLjE4MjU0MTMsMC4yNDMxNTc4OTUgMTAuMjEwNDE1MSwxLjMzMzg0MDUzIDkuMDMyNTk0NjIsMy4wNjY4ODg0IEM4LjU1NzMzMzcsMi44NjExMzA3OCA4LjA0MjM5ODgzLDIuNzUyMzk3MDkgNy41MTkxOTg1NiwyLjc1MjM5NzA5IEM1LjUxMzE4NDIyLDIuNzUyMzk3MDkgMy44NTU5NzAwOCw0LjI5MzA2OTk1IDMuNjQyNzIyNTcsNi4yNjYxNjgzNyBDMS42MDE5OTM1Myw3LjA1NzQxNTEzIDAuMjEwMDk4OTUxLDkuMDM4MDQxMjYgMC4yMTAwOTg5NTEsMTEuMzIzOTU4MiBDMC4yMTAwOTg5NTEsMTQuMzIxNjYyNiAyLjYyMDI5MTcsMTYuNzYwNjQzMSA1LjU4MjYxMzY0LDE2Ljc2MDY0MzEgTDkuNjYzMjQ1MTksMTYuNzYwNjQzMSBMOS42NjMyNDUxOSwxNS41MDYwMjM1IEw1LjU4MTc4NzEsMTUuNTA2MDIzNSBDMy4yOTg4ODE2MywxNS41MDYwMjM1IDEuNDQ5MDgzNDksMTMuNjM2NjQwMyAxLjQ0OTA4MzQ5LDExLjMyMzk1ODIgQzEuNDQ5MDgzNDksOS4yNTA0OTAxOCAyLjk0OTI1NDksNy41MzkxODkwNSA0LjkwOTgwOTUsNy4yMTIxNTE1NCBDNC44NzY3NDc4Nyw3LjA0NTcwNTM0IDQuODU4NTYzOTcsNi44NzUwNzcwOCA0Ljg1ODU2Mzk3LDYuNjk5NDMwMzMgQzQuODU4NTYzOTcsNS4yMTE0NTE0OSA2LjA0OTYwOTE1LDQuMDA3MDE2NjggNy41MTkxOTg1Niw0LjAwNzAxNjY4IEM4LjI5ODYyNjQ2LDQuMDA3MDE2NjggOC45OTI5MjA2Niw0LjM1MjQ1NTI4IDkuNDc5NzUzMTUsNC44OTI3NzgxMiBDMTAuMjE0NTQ3OSwyLjkwOTY0Mjc0IDEyLjA5ODIzNDIsMS40OTc3Nzc0OSAxNC4zMTI1MzY4LDEuNDk3Nzc3NDkgQzE3LjE2NTc1NTMsMS40OTc3Nzc0OSAxOS40Nzg0MTYzLDMuODM3MjI0ODMgMTkuNDc4NDE2Myw2LjcyNTM1OTE0IEMxOS40Nzg0MTYzLDYuODkwOTY4OTMgMTkuNDcxODAzOSw3LjA1NjU3ODcxIDE5LjQ1NDQ0NjYsNy4yMTg4NDI4NSBDMjEuMzc2OTgwMyw3LjU3ODUwMDQ2IDIyLjgzNjY1MTIsOS4yNzU1ODI1NyAyMi44MzY2NTEyLDExLjMyMzk1ODIgQzIyLjgzNjY1MTIsMTMuNjM2NjQwMyAyMC45ODY4NTMxLDE1LjUwNjAyMzUgMTguNzAzOTQ3NiwxNS41MDYwMjM1IEwxNS4wODc4MzE5LDE1LjUwNjAyMzUgTDE1LjA4NzgzMTksMTYuNzYwNjQzMSBMMTguNzAzOTQ3NiwxNi43NjA2NDMxIEMyMS42NjYyNjk2LDE2Ljc2MDY0MzEgMjQuMDc2NDYyMywxNC4zMjE2NjI2IDI0LjA3NjQ2MjMsMTEuMzIzOTU4MiBDMjQuMDc1NjM1OCw5LjA3MzE3MDYxIDIyLjcwNzcxMDksNy4wOTY3MjY1NCAyMC43MDMzNDk2LDYuMjg0NTY5NDUgTDIwLjcwMzM0OTYsNi4yODQ1Njk0NSBaIiBpZD0iU2hhcGUiPjwvcGF0aD4KICAgICAgICAgICAgICAgICAgICA8L2c+CiAgICAgICAgICAgICAgICA8L2c+CiAgICAgICAgICAgIDwvZz4KICAgICAgICA8L2c+CiAgICA8L2c+Cjwvc3ZnPg==");
            background-position: 0 0;
            content: " ";
            display: inline-block;
            height: 21px;
            margin-left: -31px;
            position: absolute;
            width: 24px;
        }
        /*For the Stikify Div*/
        .stickyDivCss {
            background: #bada55;
            color: white;
            padding: 2px;
            font-weight: bold;
            text-shadow: 0 1px 1px rgba(0,0,0,.2);
            width: 100%;
            box-sizing: border-box;
            z-index: 1 !important;
        }
    </style>
    <script type="text/javascript">
        //On page load
        $(intialLoadJqueryPlugins);

        //On UpdatePanel Refresh
        function pageLoad(sender, args) {
            if (args.get_isPartialLoad()) {
                //Specific code for partial postbacks can go in here.
                intialLoadJqueryPlugins();
            }
        }
        //for dynamic CSS of Sticky Div. Applies CSS only when scrolled down.
        $(window).scroll(function () {
            if ($(this).scrollTop() > 100) {
                $("#stickyDiv").addClass("stickyDivCss");
            }
            else {
                $("#stickyDiv").removeClass("stickyDivCss");
            }
        });

        //make the gridview sortable
        function intialLoadJqueryPlugins() {
            //Stikify the Search Panel.            
            $("#stickyDiv").sticky({ topSpacing: 0 });
            // For Copy paste image thing
            configCopyPasteImgUpload();
            //For the Reorder grid.
            $("[id*=GridReqReorder]").sortable({
                items: 'tr:not(tr:first-child)',
                cursor: 'crosshair',
                axis: 'y',
                start: function (e, ui) {
                    ui.item.addClass("selected");
                },
                stop: function (e, ui) {
                    ui.item.removeClass("selected");
                    var newOrder = '';
                    $('input[name=ObjectPKId]').each(function () {
                        newOrder = newOrder + $(this).val() + ',';
                    });

                    autoUpdateGrid(newOrder.substring(0, newOrder.length - 1));
                },
                receive: function (e, ui) {
                    $(this).find("tbody").append(ui.item);
                }
            });
            //For the row not to shrink while dragging
            $('td').each(function () {
                $(this).css('width', $(this).width() + 'px');
            });
        }

        function autoUpdateGrid(newOrder) {
            $.ajax({
                url: 'Requirements.aspx/AutoUpdateGrid',
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{'newOrder' :'" + newOrder + "'}",
                success: function (response) {
                    // .. do something
                    //window.location.reload(true);
                    UpdateUpdatePanel();
                },
                error: function (jqXHR, textStatus, errorMessage) {
                    //console.log(errorMessage); // Optional
                    $('#<%=lblError.ClientID%>').html(errorMessage);
                    //alert(errorMessage);
                }
            });
                return false;
            }
            /* function to reorder the dragged list in the grid. */
            function UpdateUpdatePanel() {
                __doPostBack("<%=upModalReorder.UniqueID %>", "FromJavaScript");
            }
            /* Script for LABELs - USEd SELECT2 jquery pligin .. check the website for documentation */
            /* fucntion to pre load the labels for ths requirement when the edit link is clicked with  show details is checked */
            /* this javascripot is called from code behind register startup script.. check the code behind for details*/
            /*  this will call the database as an ajax call and get the labels for the req and binds them when the popup is shwow */
            function initBindLabels(projGuid) {
                $.ajax({
                    type: "POST",
                    url: 'Requirements.aspx/GetRequrementLabels',
                    contentType: "application/json; charset=utf-8",
                    //data: "{'currentReqID' :'{d17a1331-2274-4f68-ad37-e46c18e20318}'}",
                    data: "{'currentReqID' :'" + projGuid + "'}",
                    success: function (msg) {
                        var data = JSON.parse(msg.d);
                        for (i = 0; i < data.length; i++) {
                            $('#select2Labelddl').append($("<option/>", {
                                value: data[i].text,
                                text: data[i].text,
                                selected: true
                            }));
                            configSelect2Labelddl(projGuid);
                        }
                        if (data.length == 0) {
                            //To configure when no results were found. 
                            configSelect2Labelddl(projGuid);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {

                        if (errorThrown != 'abort') {
                            // Do something here with the error
                            $('#<%=lblError.ClientID%>').html(errorMessage);
                        alert(textStatus);
                        alert(errorThrown);
                    }
                }
            });
            /* bind delete tag and create tag events for the Label ddl (select2) so that we can do ajax post for updates*/
            var $eventSelect = $("#select2Labelddl");
            $eventSelect.on("select2:select", function (e) { UpdateReqLabel("select", projGuid, e); });
            $eventSelect.on("select2:unselect", function (e) { UpdateReqLabel("unselect", projGuid, e); });
        }
        /* function to configure the select2 dropwdown for labels after we load the intial values. */
        function configSelect2Labelddl(projGuid) {
            // the projGuid is passed jsut to save time in retriveing the projGuid again when select/unselect happens in the label ddl.
            //the configuration required to make mutiple selects and add/remove tags for the label dropdown.
            // it has a ajax call to get the list for dropdown when user starts to enter something in the lables ddl.
            $("#select2Labelddl").select2({
                tags: "true",
                multiple: "true",
                placeholder: "Enter Labels",
                minimumInputLength: 1,
                width: "resolve",
                tokenSeparators: [','],
                ajax: {
                    //How long the user has to pause their typing before sending the next request
                    // for some reason this stupid delay isn't working as expected.. but who cares.
                    quietMillis: 35000,
                    type: "POST",
                    url: 'Requirements.aspx/GetAllUniqueLabels',
                    contentType: "application/json; charset=utf-8",
                    data: function (params) {
                        return "{'searchText' :'" + params.term + "'}";
                    },
                    processResults: function (msg) {
                        //alert(msg.d);
                        return { results: JSON.parse(msg.d) };
                    },
                    error: function (jqXHR, textStatus, errorThrown) {

                        if (errorThrown != 'abort') {
                            // Do something here with the error
                            $('#<%=lblError.ClientID%>').html(errorMessage);
                            alert(textStatus);
                            alert(errorThrown);
                        }
                    }
                }
            });
        }
        /* event that will be fired when a tag is created/selected or removed./unselected */
        function UpdateReqLabel(name, projGuid, evt) {
            // the projGuid is passed jsut to save time in retriveing the projGuid again when select/unselect happens in the label ddl.
            //alert(name);
            var curLabel = '';
            if (!evt) {
                var args = "{}";
            }
            else {
                var args = JSON.stringify(evt.params, function (key, value) {
                    if (value && value.nodeName) return "[DOM node]";
                    if (value instanceof $.Event) {
                        return "[$.Event]";
                    }
                    if (key == 'data') {
                        curLabel = value.text;
                        //alert(curLabel);
                        //alert(value.id);
                        // var str = JSON.stringify(value);
                        // var obj = JSON.parse(str);
                        // curLabel = obj.text;
                    }
                    return value;
                });
            }
            //Ajax post data to remove or add the label to the reqguid.
            $.ajax({
                type: "POST",
                url: 'Requirements.aspx/UpdateRequrementLabels',
                contentType: "application/json; charset=utf-8",
                //data: "{'currentReqID' :'{d17a1331-2274-4f68-ad37-e46c18e20318}'}",
                data: "{'currentReqID' :'" + projGuid + "', 'actionEvent' :'" + name + "', 'tagName' :'" + curLabel + "'}",
                success: function (response) {
                    // .. do something .. nothin to do for now
                    //alert(response.d);
                },
                error: function (jqXHR, textStatus, errorMessage) {
                    //console.log(errorMessage); // Optional
                    $('#<%=lblError.ClientID%>').html(errorMessage);
                    alert(errorMessage);
                }
            });
            }

            /* Function  for auto post on browse select file upload - Image*/
            function OnBrowseSelectedFiles(allFiles, eguid) {
                var data = new FormData();
                for (var i = 0; i < allFiles.length; i++) {
                    data.append(allFiles[i].name, allFiles[i]);
                }
                //alert(data);
                PostFilesData(data, eguid);
            }
            /*  Script for drag and drop images to the bordered div.. */
            function OnImageDragEnter(e) {
                e.stopPropagation();
                e.preventDefault();
            }

            function OnImageDragOver(e) {
                e.stopPropagation();
                e.preventDefault();
            }

            var selectedFiles;
            function OnImageDrop(e, eguid) {
                e.stopPropagation();
                e.preventDefault();
                selectedFiles = e.dataTransfer.files;
                //alert(selectedFiles.length + " file(s) selected for uploading!" + eguid);
                var data = new FormData();
                for (var i = 0; i < selectedFiles.length; i++) {
                    data.append(selectedFiles[i].name, selectedFiles[i]);
                }
                //alert(data);
                PostFilesData(data, eguid);
            }

            //Functio to post data to sql server.. it accepsts form data as input where it contains just the File(s).
            function PostFilesData(inFormData, reqGuid) {
                $.ajax({
                    type: "POST",
                    url: "Handlers/ImageHandler.ashx?ObjectPKID=" + reqGuid,
                    contentType: false,
                    processData: false,
                    data: inFormData,
                    success: function (result) {
                        alert(result);
                        //Reload to the same page to get the uploaded image in the datalist.
                        window.location.replace('<%= Page.ResolveUrl("~/Requirements.aspx") %>');
                    },
                    error: function (jqXHR, textStatus, errorMessage) {
                        //console.log(errorMessage); // Optional
                        $('#<%=lblError.ClientID%>').html(errorMessage);
                    alert(errorMessage);
                }
                });
            }
    </script>
    <script type="text/javascript">
        var blobdataUrl;
        var blobPacket;
        function configCopyPasteImgUpload() {
            $('.demo-noninputable').pastableNonInputable();
            $('.demo').on('pasteImage',
                            function (ev, data) {
                                //blobUrl = URL.createObjectURL(data.blob);
                                $(this).replaceWith($('<div class="result">'
						                             + '<img  height="350px" width="350px" src="' + data.dataURL + '" > '
                                                     + '</div>'));
                                blobdataUrl = data.dataURL;
                                blobPacket = data.blob;
                                //uploadFile(data.dataURL, data.blob);
                            }
						);
        }
        function uploadCopyPastedFile() {
            if (blobdataUrl) {
                if ($('#<%=txtImageName.ClientID%>').val() == "") {
                    $('#<%=lblResultUpload.ClientID%>').html('*Please Enter Text for Image');
                    return false;
                }
                else {
                    //alert(blobPacket.size);
                    uploadFile(blobdataUrl, blobPacket, $('#<%=txtImageName.ClientID%>').val(), $('#<%=hfImageObjectPK.ClientID%>').val());

                    return false;
                }

            }
            else {
                // no data send error message.
                $('#<%=lblResultUpload.ClientID%>').html('*Please Paste image');
                return false;
            }
        }

        function uploadFile(based64Binary, blobPacket, fileName, ObjectPK) {

            $.ajax({
                url: 'Requirements.aspx/UploadCopyPasteImage',
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{ 'Based64BinaryString' :'" + based64Binary + "' , 'dataType' : '" + blobPacket.type + "', 'dataSize' : " + blobPacket.size +
                        " , 'fileName' : '" + fileName + "' , 'objectID' : '" + ObjectPK + "' }",
                success: function (response) {
                    //alert(response.d);
                    $('#<%=lblResultUpload.ClientID%>').html(' Image Sucessfully Uploaded');
                    //$find('mpCopyPasteImg').hide();
                    $('#<%=btnCopyPasteUploadImage.ClientID%>').prop('disabled', true);
                    $('#<%=btnCopyPasteCancel.ClientID%>').val('Close');
                    $('#<%=txtImageName.ClientID%>').attr("disabled", "disabled");
                    $('#<%=lblResultUpload.ClientID%>').css('color', 'Green')
                },
                error: function (jqXHR, textStatus, errorMessage) {
                    //console.log(errorMessage); // Optional
                    alert(errorMessage);
                }
            });
            //return false;
        }
        function cancelUpload() {
            $('.result').replaceWith($('<div class="demo demo-noninputable pastable">Your Image will be posted here.</div>'));
            configCopyPasteImgUpload();
            return false;
        }
        //For notificatios.
        function DisplayNotification(inMsg, inType) {
            notif({
                msg: inMsg,
                type: inType,
                position: "center",
                multiline: true
            });
        }
    </script>
    <style type="text/css">
        /* css for draggable table on the popup.*/
        .ui-sortable-helper {
            display: table;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="welcomeMessage">
        <asp:Label ID="lblPackageName" runat="server">
        </asp:Label>
    </div>
    <asp:MultiView ID="mvRequirements" runat="server">
        <asp:View ID="vwNoSelect" runat="server">
            <br />
            <br />
            <span>No Package is selected. Please navigate to
                <asp:HyperLink ID="lnkPackages" runat="server" NavigateUrl="~/Packages.aspx" Text="Packages"></asp:HyperLink>
                Page and select a package. </span>
        </asp:View>
        <asp:View ID="vwDefault" runat="server">
            <asp:UpdatePanel ID="updatePanelReq" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnReferesh">
                        <table width="100%">
                            <tr>
                                <td align="left" style="padding-bottom: 5px; padding-top: 5px;">
                                    <span style="font-weight: bold">Requirements </span>
                                </td>
                                <td align="right" style="padding-bottom: 5px; padding-top: 5px;">
                                    <CustomButtonDropDown:ButtonDropDown ID="btnDropDownReports" runat="server" />
                                    &nbsp;                                    
                                    <asp:Button ID="btnReorderReq" runat="server" Text="Reorder" CssClass="customButton"
                                        OnClick="btnReorderReq_Click" />
                                    &nbsp;
                                    <asp:Button ID="btnAddRequirement" runat="server" Text="Add Requirement" CssClass="customButton"
                                        OnClick="btnAddRequirement_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <div id="stickyDiv">
                                        <span>Search</span>&nbsp;
                                        <asp:TextBox ID="txtSearchReq" runat="server" Width="250px"></asp:TextBox>
                                        &nbsp;&nbsp;
                                        <asp:CheckBox ID="chkshowDetails" runat="server" Text="Show Details" TextAlign="Left" />
                                        &nbsp;&nbsp;
                                        <asp:CheckBox ID="chkShowHiddenReq" runat="server" Text="Show Hidden" TextAlign="Left" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnReferesh" runat="server" Text="Refresh" CssClass="customButton"
                                            OnClick="btnReferesh_Click" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:DataList ID="DataListRequirements" runat="server" Width="100%" CellPadding="4"
                        OnItemCommand="DataListRequirements_ItemCommand" OnItemDataBound="DataListRequirements_ItemDataBound">
                        <ItemTemplate>
                            <table style="border: 1; width: 100% !important;">
                                <tr>
                                    <td>
                                        <div style="border: 1px solid #d3d3d3 !important; width: 50px; height: 20px; text-align: center; vertical-align: middle;">
                                            <%# Eval("Req_ID")%>
                                        </div>
                                    </td>
                                    <td style="width: 90%;">
                                        <div style="border-top-style: solid; border-top-width: medium;">
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnHideUnhideReq" ToolTip="Hide/Unhide requirement" runat="server" CommandArgument='<%# Eval("ea_guid") %>'
                                            CssClass="customButton" Text='<%# Convert.ToInt32(Eval("HiddenFlag")) == 0 ? "Hide" : "UnHide" %>' Width="70px"
                                            UseSubmitBehavior="false" CommandName='<%# Convert.ToInt32(Eval("HiddenFlag")) == 0 ? "HideRequirement" : "UnHideRequirement" %>'></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnEditProject" ToolTip="Edit" runat="server" CommandArgument='<%# Eval("ea_guid") %>'
                                            CssClass="customButton" Text="Edit" Width="70px" UseSubmitBehavior="false" CommandName="EditRequirement"></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnShowHistReq" ToolTip="show history" runat="server" CommandArgument='<%# Eval("ea_guid") %>'
                                            CssClass="customButton" Text="History" Width="70px" UseSubmitBehavior="false"
                                            CommandName="ShowHistoryRequirement"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                            <table style="border: 1; width: 100% !important;">
                                <tr id="trReqDetailsinList" runat="server" visible="false">
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="6" align="left" style="padding-bottom: 5px;">
                                                    <span style="font-weight: bold">
                                                        <%# Eval("Name") %>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="font-weight: bold;">Version:
                                                </td>
                                                <td>
                                                    <%# Eval("Version") %>
                                                </td>
                                                <td style="font-weight: bold;">Created Date:
                                                </td>
                                                <td>
                                                    <%# Eval("CreatedDate") %>
                                                </td>
                                                <td style="font-weight: bold;">Created By:
                                                </td>
                                                <td>
                                                    <%# Eval("FullName") %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="font-weight: bold;">Type:
                                                </td>
                                                <td>
                                                    <%# Eval("TypeIs") %>
                                                </td>
                                                <td style="font-weight: bold;">Complexity:
                                                </td>
                                                <td>
                                                    <%# Eval("Complexity") %>
                                                </td>
                                                <td style="font-weight: bold;">Status:
                                                </td>
                                                <td>
                                                    <%# Eval("Status") %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="font-weight: bold;">Labels:
                                                </td>
                                                <td colspan="5">
                                                    <%# Eval("Labels") %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <hr />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 2px;">
                                        <asp:Label ID="lblReqDetailsNameinList" Font-Bold="true" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--<div id="divText" style="overflow: auto; overflow-y: hidden; width: 850px;">--%>
                                        <asp:Literal ID="litReqTextNotes" runat="server" Text='<%# Eval("Note") %>' />
                                        <%--</div>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="image-drop-zone" id="imgDropDiv" runat="server" ondragenter="OnImageDragEnter(event)"
                                            ondragover="OnImageDragOver(event)" ondrop='<%# "OnImageDrop(event, \"" + Eval("object_PK") + "\" );" %>'>
                                            <span style="display: block; text-align: center;"><span class="image-drop-zone__drop-icon"></span>Drop Image here, or
                                                <label class="browselinkcss">
                                                    Browse
                                                    <input type="file" onchange='<%# "OnBrowseSelectedFiles(this.files, \"" + Eval("object_PK") + "\" );" %>'
                                                        style="display: none;" id="fuUploadReqImage" accept="image/*">
                                                </label>
                                                <span>or,</span>
                                                <asp:LinkButton ID="lnkBtnPasteImage" runat="server" Text="Paste Image" ToolTip="click to open popup to paste image"
                                                    CommandName="OpenCopyPasteImagePopup" CommandArgument='<%# Eval("object_PK") %>'>
                                                </asp:LinkButton>
                                            </span>
                                        </div>
                                        <div class="image-drop-zone" id="imgShowDiv" runat="server" visible="false">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 95%;">
                                                        <%-- Use the below Styles for image so that it wont take the original image size and overflow to right (width)--%>
                                                        <asp:Image ID="imgReqirementDetail" Visible="false" runat="server" Style="max-width: 800px; width: auto;" />
                                                    </td>
                                                    <td align="right" valign="bottom">
                                                        <asp:ImageButton ID="trashImg" runat="server" ToolTip="Click to Delete Image" AlternateText="trash icon"
                                                            ImageUrl="~/Images/Trash_Icon.png" Height="25px" Width="25px" CommandName="DeleteImageFromReq"
                                                            CommandArgument='<%# Eval("object_PK") %>' />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <FooterTemplate>
                            <hr />
                        </FooterTemplate>
                    </asp:DataList>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnReferesh" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnReorderReq" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnAddRequirement" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
    <br />
    <asp:Label ID="lblError" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
    <br />
    <!-- ModalPopupExtender Create Project Req  -->
    <asp:HiddenField runat="server" ID="hfMpCreateRequirement" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpCreateUpdateReq" runat="server" PopupControlID="PanelCreateUpdateReq"
        TargetControlID="hfMpCreateRequirement" CancelControlID="btnCreateUpdateReqClose"
        BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelCreateUpdateReq" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 65%;">
        <asp:UpdatePanel runat="server" ID="upCreateUpdateReq" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hfCurentReqId" runat="server" />
                <table width="100%" id="tblRequirement" runat="server">
                    <tr id="trReqDetails" runat="server" visible="true">
                        <td>
                            <table width="100%">
                                <tr>
                                    <td align="right" style="font-weight: bold;">Title:
                                    </td>
                                    <td colspan="5" align="left">
                                        <asp:TextBox ID="txtReqTitle" runat="server" Width="400px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trReqAdditionals2" runat="server" visible="false">
                                    <td align="right" style="font-weight: bold;">Version:
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblReqVersion" runat="server"></asp:Label>
                                    </td>
                                    <td align="right" style="font-weight: bold;">Create Date:
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblReqCreatedDate" runat="server"></asp:Label>
                                    </td>
                                    <td align="right" style="font-weight: bold;">Created By:
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblReqCreatedBy" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trReqAdditionals" runat="server" visible="false">
                                    <td align="right" style="font-weight: bold;">Type:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlReqType" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right" style="font-weight: bold;">Complexity:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlReqComplexity" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right" style="font-weight: bold;">Status:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlReqStatus" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trReqLabels" runat="server" visible="false">
                                    <td align="right" style="font-weight: bold;">Label:
                                    </td>
                                    <td colspan="5" align="left">
                                        <%--<asp:DropDownList ID="ddlReqLabel" runat="server">
                                </asp:DropDownList>--%>
                                        <select id="select2Labelddl" style="width: 500px;">
                                        </select>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <CKEditor:CKEditorControl Height="200px" ID="htmlEditorCreateUpdateReqNotes" BasePath="~/Plugins/ckeditor/"
                                runat="server"></CKEditor:CKEditorControl>
                        </td>
                    </tr>
                </table>
                &nbsp;&nbsp;<asp:Label ID="lblErrorCreateUpdateReq" runat="server" Font-Bold="true"
                    ForeColor="Green"></asp:Label>
                <br />

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnCreateUpdateReq" />

            </Triggers>
        </asp:UpdatePanel>
        &nbsp;&nbsp;
                <asp:Button ID="btnCreateUpdateReq" runat="server" Text="Update" CssClass="customButton"
                    OnClick="btnCreateUpdateReq_Click" />
        &nbsp;&nbsp;<asp:Button ID="btnCreateUpdateReqClose" runat="server" Text="Close"
            CssClass="customButton" />
    </asp:Panel>
    <!-- End ModalPopupExtender Create Project Req  -->
    <!-- ModalPopupExtender Reoorder Requirements -->
    <asp:HiddenField runat="server" ID="hrfMp2" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpReorderReq" runat="server" PopupControlID="PanelReporderReq"
        TargetControlID="hrfMp2" CancelControlID="hrfMp2" BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelReporderReq" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 60%;">
        <asp:HiddenField ID="HiddenField22" runat="server" />
        <span style="font-weight: bold;">Reorder Requirements</span>
        <br />
        <span>Drag and drop the reqirements so they are in the order you prefrer</span>
        <hr />
        <asp:UpdatePanel runat="server" ID="upModalReorder" UpdateMode="Conditional" OnLoad="upModalReorder_Load">
            <ContentTemplate>
                <div style="width: 100%; height: auto !important; max-height: 400px; overflow: auto;">
                    <asp:GridView ID="GridReqReorder" runat="server" Width="100%" UseAccessibleHeader="True"
                        GridLines="None" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false"
                        AllowPaging="false" CssClass="table table-striped table-bordered table-condensed"
                        DataKeyNames="Object_PK,Name,TPos">
                        <Columns>
                            <asp:TemplateField Visible="true" AccessibleHeaderText="Name" HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblReorderObjectName" runat="server" Text='<%#  Eval("Object_PK").ToString() + " - " + Eval("Name").ToString() %>' />
                                    <input type="hidden" name="ObjectPKId" value='<%# Eval("Object_PK") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            Sorry.! nothing to Display
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        &nbsp;&nbsp;<asp:Button ID="btnCloseReorderReq" runat="server" Text="Close" CssClass="customButton"
            OnClick="btnCloseReorderReq_Click" />
    </asp:Panel>
    <!-- End ModalPopupExtender Re order Req  -->
    <!-- Modal Popup Copy paste Image  upload -->
    <asp:HiddenField runat="server" ID="hrfMp3" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpCopyPasteImg" runat="server" PopupControlID="PanelCopyPasteImgUpload"
        TargetControlID="hrfMp3" CancelControlID="hrfMp3" BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelCopyPasteImgUpload" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 60%;">
        <asp:HiddenField ID="hfImageObjectPK" runat="server" />
        <div style="background-color: White !important;">
            <table width="50%">
                <tr>
                    <td align="right">Take a screenshot:
                    </td>
                    <td style="padding-left: 10px;">
                        <span class="spanButton">PrtScn</span>
                    </td>
                </tr>
                <tr>
                    <td align="right">Paste the image below:
                    </td>
                    <td style="padding-left: 10px;">
                        <span class="spanButton">Ctrl</span> + <span class="spanButton">v</span>
                    </td>
                </tr>
            </table>
            <div class="demo demo-noninputable pastable">
                Your Image will be posted here.
            </div>
            <br />
            <asp:Panel ID="pnlSubmitImage" runat="server" DefaultButton="btnCopyPasteUploadImage">
                <table width="100%">
                    <tr>
                        <td align="right">File Name:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtImageName" Height="130%" runat="server" Width="200px" MaxLength="20"
                                AutoCompleteType="Search" Text="YourFile-1" CausesValidation="true" ValidationGroup="valGroupImg"></asp:TextBox>
                            <asp:Label ID="lblResultUpload" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="padding-top: 10px;"></td>
                        <td style="padding-left: 10px; padding-top: 10px;">
                            <asp:Button ID="btnCopyPasteUploadImage" runat="server" CssClass="customButton" ToolTip="click to upload"
                                Text="Upload" OnClientClick="return uploadCopyPastedFile();" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnCopyPasteCancel" runat="server" CssClass="customButton" Text="Cancel"
                                ToolTip="cancel upload" OnClick="btnCopyPasteCancel_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </asp:Panel>
    <!-- END ModalPopupExtender Copy Paste Image Upload  -->
    <!-- ModalPopupExtender View Package History -->
    <asp:HiddenField runat="server" ID="hfReqHist" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpViewReqHist" runat="server" PopupControlID="pnlViewReqHist"
        TargetControlID="hfReqHist" CancelControlID="btnCloseReqHist" BackgroundCssClass="modalBackground"
        Drag="true">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlViewReqHist" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 60%;">
        <asp:HiddenField ID="hfCurrentReqIdHist" runat="server" />
        <span style="font-weight: bold; text-align: left; padding-right: 10px !important;">Req
            Id:</span>
        <asp:Label ID="lblCurrentReqIDHist" runat="server"></asp:Label>
        &nbsp;&nbsp;-
        <asp:Label ID="lblReqNameHist" runat="server" Width="350px"></asp:Label>
        <hr />
        <asp:UpdatePanel runat="server" ID="upModalReqHist" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="GridReqHist" runat="server" Width="100%" UseAccessibleHeader="True"
                    GridLines="None" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false"
                    AllowPaging="true" PageSize="10" CssClass="table table-striped table-bordered table-condensed"
                    DataKeyNames="Version,ea_guid,CreatedDate,StatusName" OnRowCommand="GridReqHist_RowCommand"
                    OnPageIndexChanging="GridReqHist_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField AccessibleHeaderText="Version" HeaderText="Version" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkReqViewHist" runat="server" Text='<%# Eval("Version") %>'
                                    ToolTip="Click to see more information" CommandName="ViewReqHistInfo" CommandArgument='<%# Container.DataItemIndex %>'> 
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField AccessibleHeaderText="Created Date" HeaderText="Created Date" DataField="CreatedDate"
                            SortExpression="CreatedDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField AccessibleHeaderText="Status" HeaderText="Status" DataField="StatusName"
                            SortExpression="StatusName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField AccessibleHeaderText="Created By" HeaderText="Created By" DataField="CreatedByName"
                            SortExpression="CreatedByName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    </Columns>
                    <EmptyDataTemplate>
                        Sorry.! nothing to Display
                    </EmptyDataTemplate>
                </asp:GridView>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        &nbsp;&nbsp;<asp:Button ID="btnCloseReqHist" runat="server" Text="Close" CssClass="customButton" />
    </asp:Panel>
    <!-- End ModalPopupExtender View Package History -->
    <!-- ModalPopupExtender View/Edit Package -->
    <asp:HiddenField runat="server" ID="hrfViewReq" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpViewReqDetail" runat="server" PopupControlID="pnlViewReqDetail"
        TargetControlID="hrfViewReq" CancelControlID="btnViewReqClose" BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlViewReqDetail" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 60%;">
        <table style="border: 1; width: 100% !important;">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td colspan="3" align="left" style="padding-bottom: 5px;">
                                <span style="font-weight: bold">
                                    <asp:Label ID="lblViewReqName" runat="server"></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span style="font-weight: bold">Version: </span>
                                <asp:Label ID="lblViewReqDetailsVersion" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span style="font-weight: bold">Created Date: </span>
                                <asp:Label ID="lblViewReqDetailsCreatedDate" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span style="font-weight: bold">Created By: </span>
                                <asp:Label ID="lblViewReqDetailsCreatedBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span style="font-weight: bold">Type: </span>
                                <asp:Label ID="lblViewReqDetailsType" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span style="font-weight: bold">Complexity: </span>
                                <asp:Label ID="lblViewReqDetailsComplexity" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span style="font-weight: bold">Status: </span>
                                <asp:Label ID="lblViewReqDetailsStatus" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <span style="font-weight: bold">Labels: </span>
                                <asp:Label ID="lblViewReqDetailsLabel" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divText" style="overflow: auto; height: 300px !important;">
                        <asp:Literal ID="litViewReqTextNotes" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="btnViewReqClose" runat="server" Text="Close" CssClass="customButton" />
        &nbsp;&nbsp;
        <asp:Button ID="btnViewEditReq" runat="server" Text="Edit" CssClass="customButton"
            OnClick="btnViewEditReq_Click" />
    </asp:Panel>
    <!-- End ModalPopupExtender View/Edit Package -->
    <!-- ModalPopupExtender Hide UnHide Req  -->
    <asp:HiddenField runat="server" ID="hfHideunHideReq" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpHideUnHideReq" runat="server" PopupControlID="PanelHideUnhideReq"
        TargetControlID="hfHideunHideReq" CancelControlID="hfHideunHideReq"
        BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelHideUnhideReq" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 35%;">
        <asp:UpdatePanel runat="server" ID="upHideUnHideReq" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divHideUnHideReqPopupHide" runat="server">
                    <span style="font-weight: bold;">Hide Requirement</span>
                    <br />
                    <br />
                    <span>This will hide this requirement from various requirement reports. However it will continue to be seen on the Requirements List unless
                        you filter it out.
                    </span>
                    <br />
                    <br />
                    <table width="50%">
                        <tr>
                            <td align="left" style="font-weight: bold; padding-left: 5px;">Hidden By:
                            </td>
                            <td align="left" style="padding-left: 5px;">
                                <asp:Label ID="lblHideReqPopUpUser" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="font-weight: bold; padding-left: 5px;">Date Hidden:
                            </td>
                            <td align="left" style="padding-left: 5px;">
                                <asp:Label ID="lblHideReqPopUpDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divHideUnHideReqPopupUnHide" runat="server">
                    <span style="font-weight: bold;">UnHide Requirement</span>
                    <br />
                    <br />
                    <span>This will unhide this requirement from various requirement reports. However it will always be see on the Requirements List.                        
                    </span>
                    <br />
                    <br />
                    <table width="50%">
                        <tr>
                            <td align="left" style="font-weight: bold; padding-left: 5px;">UnHidden By:
                            </td>
                            <td align="left" style="padding-left: 5px;">
                                <asp:Label ID="lblUnHideReqPopUpUser" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="font-weight: bold; padding-left: 5px;">Date UnHidden:
                            </td>
                            <td align="left" style="padding-left: 5px;">
                                <asp:Label ID="lblUnHideReqPopUpDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                &nbsp;&nbsp;<asp:Button ID="btnHideUnHideReqPopUp" runat="server" Text="Hide/UnHIde" CssClass="customButton" ToolTip="Hide unhide Requirement"
                    OnClick="btnHideUnHideReqPopUp_Click" />
                &nbsp;&nbsp;<asp:Button ID="btnHideUnHideReqClose" runat="server" Text="Cancel"
                    CssClass="customButton" OnClick="btnHideUnHideReqClose_Click"  />
            </ContentTemplate>
        </asp:UpdatePanel>
        
    </asp:Panel>
    <!-- End ModalPopupExtender Hide UnHide Req  -->
</asp:Content>
