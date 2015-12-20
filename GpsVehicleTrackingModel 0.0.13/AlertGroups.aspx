<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AlertGroups.aspx.cs" Inherits="AlertGroups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <link href="jquery.jqGrid-4.5.2/ui.Jquery.css" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <link href="jquery.jqGrid-4.5.2/js/i18n/jquery-ui-1.9.2.custom.css" rel="stylesheet"
        type="text/css" />
    <link rel="stylesheet" type="text/css" href="../jquery.jqGrid-4.5.2/plugins/searchFilter.css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <link href="jquery.jqGrid-4.5.2/js/i18n/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <script src="jquery.jqGrid-4.5.2/src/i18n/grid.locale-en.js" type="text/javascript"></script>
    <script src="jquery.jqGrid-4.5.2/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="jquery.jqGrid-4.5.2/plugins/jquery.searchFilter.js" type="text/javascript"></script>
    <link href="jquery.jqGrid-4.5.2/js/Jquery.ui.css.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
     <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>


     <style type="text/css">
.headerDiv  
     {
         background-color:rgb(231, 170, 170);
         width:100%;
         height:25px;
}
.SubheaderDiv
{
    background-color:rgb(231, 170, 170);
         width:90%;
         height:20px;
         margin-left: 5%;
}
.follwingDiv
{
    width:100%;
    height:100px;
    background-color:rgb(243, 229, 229);
    overflow:auto;
}
#ldiv {
    height: 100%;
    width: 16%;
    position:relative;
    float:left;
    background-color:#fff;
    color:#6D5353;
    
   margin-left: 2%;
   overflow:auto;
}

#rdiv {
    vertical-align: top; position: relative;width: 50%;float: left;margin-left: 2%;
}
#btndiv
{
    position: relative;float: left;margin-left: 2%;margin-top:20%;
}


@media only screen and (max-width: 960px) 
{
    #ldiv {
    height: 100%;
    width: 16%;
    position:relative;
    float:left;
    background-color:#fff;
    color:#ccc;
    
   margin-left: 2%;
}


#rdiv {
    vertical-align: top; position: relative;width: 50%;float: left;margin-left: 2%;
}
#btndiv
{
    position: relative;float: left;margin-left: 2%;margin-top:20%;
}

}

    
    .divselectedclass
    {
        border:1px solid gray;
        padding-top:2px;
        padding-bottom:2px;
    }
     .back-red { background-color: #ffffcc; }
     .back-white { background-color: #ffffff; }
     
     .unitline
        {
            font: inherit;
            width: 120px;
        }
        .iconminus
        {
            float: right;
            width: 20px;
            height: 20px;
            margin: 2px 0 0 0;
            background: url("Images/minus.png") no-repeat;
            border-radius: 2px 2px 2px 2px;
        }
        .titledivcls
        {
            height: 30px;
        }
        .divcategory
        {
            border-bottom-style: dashed;
            border-bottom-color: #D6D6D6;
            border-bottom-width: 1px;
        }
           .activeanchor
        {
            text-decoration: none;
            color: #000000;
        }
    .titleText {
float: left;
font-size: 1.1em;
font-weight: bold;
/*margin: 5px; */
margin-left: 20px;
color:Yellow;
}
 .checkbox {

font-size: 1.1em;
font-weight: bold;
margin: 3px;
color:Yellow;
}
.subcheckbox {

font-size: 1.1em;
font-weight: bold;
margin: 3px;
color:Yellow;
}
.subtitleText 
{
    float:left;
font-size: 1.1em;
font-weight: bold;
margin: 1px;
margin-left: 20px;
color:Yellow;
}
.btnstyle
{
    background-color: blanchedalmond;
}

#tablestpinpoishow {
    border-collapse: collapse;
}

#tablestpinpoishow td  {
    border: 1px solid black;
}
#tablestpinpoishow  th {
    border: 1px solid black;
}
#tablestpinpoishow tr:hover {
    background-color: #ccc;
}

.btn {
  background: #39a5ed;
  background-image: -webkit-linear-gradient(top, #39a5ed, #2980b9);
  background-image: -moz-linear-gradient(top, #39a5ed, #2980b9);
  background-image: -ms-linear-gradient(top, #39a5ed, #2980b9);
  background-image: -o-linear-gradient(top, #39a5ed, #2980b9);
  background-image: linear-gradient(to bottom, #39a5ed, #2980b9);
  -webkit-border-radius: 5;
  -moz-border-radius: 5;
  border-radius: 5px;
  font-family: Arial;
  color: #ffffff;
  font-size: 13px;
  padding: 10px 20px 10px 20px;
  border: solid #45667a 0px;
  text-decoration: none;
}

.btn:hover {
  background: #3cb0fd;
  background-image: -webkit-linear-gradient(top, #3cb0fd, #0f3e5e);
  background-image: -moz-linear-gradient(top, #3cb0fd, #0f3e5e);
  background-image: -ms-linear-gradient(top, #3cb0fd, #0f3e5e);
  background-image: -o-linear-gradient(top, #3cb0fd, #0f3e5e);
  background-image: linear-gradient(to bottom, #3cb0fd, #0f3e5e);
  text-decoration: none;
}

    </style>

    <script type="text/javascript">

        $(function () {
            forspeeddiv();
            forinpoidiv();
            foroutpoidiv();
            forstoppagediv();
            //forradios1();
            FillBranches();
            retrievealrtnme();
            $('#tablestpinpoishow').css('cursor', 'pointer');
        });

        $(function () {

            $('#txt_search').keyup(function () {

                var searchText = $(this).val();

                $('ul>li').each(function () {

                    var currentLiText = $(this).text(),
                showCurrentLi = currentLiText.indexOf(searchText) !== -1;

                    $(this).toggle(showCurrentLi);

                });
            });

        });


        function callHandler(d, s, e) {
            $.ajax({
                url: 'Bus.axd',
                data: d,
                type: 'GET',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
        function CallHandlerUsingJson(d, s, e) {
            $.ajax({
                type: "GET",
                url: "Bus.axd?json=",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(d),
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
        var speedchk = "";
        var inpoichk = "";
        var outpoichk = "";
        var stoppagechk = "";
        var stpinpoichk = "";
        var stpoutpoichk = "";
        var mainpowerchk = "";
        //var stopnoalertchk = "";
        function forspeeddiv() {
            if (document.getElementById('chk_speedenable').checked == true) {
                //$('#speeddiv').show();
                var nodes = document.getElementById("speeddiv").getElementsByTagName('*');
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].disabled = false;
                }
                speedchk = "speed";
                $('#speeddiv').show();

            }
            else {
                // $('#speeddiv').hide();
                var nodes = document.getElementById("speeddiv").getElementsByTagName('*');
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].disabled = true;
                }
                speedchk = "";
                $('#speeddiv').hide();
            }
        }
        function forinpoidiv() {
            if (document.getElementById('chk_inpoienable').checked == true) {
                //$('#speeddiv').show();
                var nodes = document.getElementById("inpoidiv").getElementsByTagName('*');
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].disabled = false;
                }
//                var nodes2 = document.getElementById("divchblroutes").getElementsByTagName('*');
//                for (var j = 0; j < nodes2.length; j++) {
//                    nodes2[j].disabled = false;
//                }
                inpoichk = "inpoi";
                $('#inpoidiv').show();

            }
            else {
                // $('#speeddiv').hide();
                var nodes = document.getElementById("inpoidiv").getElementsByTagName('*');
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].disabled = true;
                }
//                var nodes2 = document.getElementById("divchblroutes").getElementsByTagName('*');
//                for (var j = 0; j < nodes2.length; j++) {
//                    nodes2[j].disabled = true;
//                }
                inpoichk = "";
                $('#inpoidiv').hide();

            }
        }
        function foroutpoidiv() {
            if (document.getElementById('chk_outpoienable').checked == true) {
                //$('#speeddiv').show();
                var nodes = document.getElementById("outpoidiv").getElementsByTagName('*');
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].disabled = false;
                }
//                var nodes2 = document.getElementById("divchblroutes").getElementsByTagName('*');
//                for (var j = 0; j < nodes2.length; j++) {
//                    nodes2[j].disabled = false;
//                }
                outpoichk = "outpoi";
                $('#outpoidiv').show();

            }
            else {
                // $('#speeddiv').hide();
                var nodes = document.getElementById("outpoidiv").getElementsByTagName('*');
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].disabled = true;
                }
//                var nodes2 = document.getElementById("divchblroutes").getElementsByTagName('*');
//                for (var j = 0; j < nodes2.length; j++) {
//                    nodes2[j].disabled = true;
//                }
                outpoichk = "";
                $('#outpoidiv').hide();

            }
        }
        function forstoppagediv() {
            if (document.getElementById('chk_stoppageenable').checked == true) {
                //$('#speeddiv').show();
                var nodes = document.getElementById("stoppagediv").getElementsByTagName('*');
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].disabled = false;
                }
                stoppagechk = "stoppage";
                $('#stoppagediv').show();
                forstopinpoi();
                forstopoutpoi();
                //forstopnoalert();
            }
            else {
                // $('#speeddiv').hide();
                var nodes = document.getElementById("stoppagediv").getElementsByTagName('*');
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].disabled = true;
                }
                stoppagechk = "";
                $('#stoppagediv').hide();
                forstopinpoi();
                forstopoutpoi();
                //forstopnoalert();
            }
        }

        function forstopinpoi() {
            if (document.getElementById('chk_stpinpoi').checked == true) {
                //$('#speeddiv').show();
                var nodes = document.getElementById("stopinpoidiv").getElementsByTagName('*');
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].disabled = false;
                }
                stpinpoichk = "stopinpoi";
                $('#stopinpoidiv').show();

            }
            else {
                // $('#speeddiv').hide();
                var nodes = document.getElementById("stopinpoidiv").getElementsByTagName('*');
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].disabled = true;
                }
                stpinpoichk = "";
                $('#stopinpoidiv').hide();
            }

        }

        function forstopoutpoi() {
            if (document.getElementById('chk_stpoutpoi').checked == true) {
                //$('#speeddiv').show();
                var nodes = document.getElementById("stopoutpoidiv").getElementsByTagName('*');
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].disabled = false;
                }
                stpoutpoichk = "stopoutpoi";
                $('#stopoutpoidiv').show();
            }
            else {
                // $('#speeddiv').hide();
                var nodes = document.getElementById("stopoutpoidiv").getElementsByTagName('*');
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].disabled = true;
                }
                stpoutpoichk = "";
                $('#stopoutpoidiv').hide();

            }

        }

        function formainalertchk() {
            if (document.getElementById('chk_mainpower').checked == true) {
                mainpowerchk = "mainpower";
            }
            else {
                mainpowerchk = "";
            }
        }

        //         function forstopnoalert() {
        //             if (document.getElementById('chk_nolaert').checked == true) {
        //                 //$('#speeddiv').show();
        //                 var nodes = document.getElementById("stopnoalertdiv").getElementsByTagName('*');
        //                 for (var i = 0; i < nodes.length; i++) {
        //                     nodes[i].disabled = false;
        //                 }
        //                 stpoutpoichk = "stopoutpoi";
        //                 $('#stopoutpoidiv').show();
        //             }
        //             else {
        //                 // $('#speeddiv').hide();
        //                 var nodes = document.getElementById("stopnoalertdiv").getElementsByTagName('*');
        //                 for (var i = 0; i < nodes.length; i++) {
        //                     nodes[i].disabled = true;
        //                 }
        //                 stpoutpoichk = "";
        //                 $('#stopoutpoidiv').hide();

        //             }
        //         }

        //---------------->Get all Locations<-------------------------//
        var allbranchesdata;
        function FillBranches() {
            var data = { 'op': 'get_Routes' };
            var s = function (msg) {
                if (msg) {
                    allbranchesdata = msg;
                    ddlselectplant_selectionchanged();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

            callHandler(data, s, e);
        }

        function ddlselectplant_selectionchanged() {

            selectedplant = "Select All";

            document.getElementById('divchblroutes').innerHTML = "";
            var plants = [];
            for (var i = 0; i < allbranchesdata.length; i++) {
                if (typeof allbranchesdata[i] === "undefined") {
                }
                else {
                    var tplantname = allbranchesdata[i].PlantName;
                    var tplantsno = allbranchesdata[i].PlantSno;

                    if (selectedplant != "Select All") {
                        if (tplantsno == selectedplant) {
                            tplantname = tplantname.replace(/[^a-zA-Z0-9]/g, '');
                            var exists = plants.indexOf(tplantname);
                            if (exists == -1) {
                                var plantname = allbranchesdata[i].PlantName;
                                plantname = plantname.replace(/[^a-zA-Z0-9]/g, '');
                                plants.push(plantname);
                                $("#divchblroutes").append("<div id='div" + plantname + "' class='divcategory' align='left'>");
                            }
                        }
                    }
                    else {
                        tplantname = tplantname.replace(/[^a-zA-Z0-9]/g, '');
                        var exists = plants.indexOf(tplantname);
                        if (exists == -1) {
                            var plantname = allbranchesdata[i].PlantName;
                            plantname = plantname.replace(/[^a-zA-Z0-9]/g, '');
                            plants.push(plantname);
                            $("#divchblroutes").append("<div id='div" + plantname + "' class='divcategory' align='left'>");
                        }
                    }
                }
            }
            for (var p = 0; p < plants.length; p++) {
                $("#div" + plants[p] + "").append("<div class='titledivcls'><table style='width:100%;'><tr><td style='width: 120px;'><h2 class='unitline'>" + plants[p] + "</h2></td><td></td><td style='padding-right: 20px;vertical-align: middle;'><span class='iconminus' title='Hide' onclick='minusclick(this);'></span></td></tr></table></div>");
                $("#div" + plants[p] + "").append("<ul id='ul" + plants[p] + "' class='ulclass'>");
                for (var i = 0; i < allbranchesdata.length; i++) {
                    var tplantname = allbranchesdata[i].PlantName;
                    tplantname = tplantname.replace(/[^a-zA-Z0-9]/g, '');
                    if (typeof allbranchesdata[i] === "undefined" || allbranchesdata[i].Name == "" || allbranchesdata[i].Name == null) {
                    }
                    else {
                        if (plants[p] == tplantname) {
                            var label = document.createElement("span");
                            var hidden = document.createElement("input");

                            var list = document.createElement("li");
                            list.setAttribute("id", allbranchesdata[i].Name)
                            document.getElementById('ul' + plants[p]).appendChild(list);


                            hidden.type = "hidden";
                            hidden.name = "hidden";
                            hidden.value = allbranchesdata[i].id;
                            var checkbox = document.createElement("input");
                            checkbox.type = "checkbox";
                            checkbox.name = "checkbox";
                            checkbox.value = "checkbox";
                            checkbox.id = "checkbox";
                            checkbox.className = 'chkclass';
                            checkbox.onclick = 'checkclick();';
                            // document.getElementById('ul' + plants[p]).appendChild(checkbox);
                            document.getElementById(allbranchesdata[i].Name).appendChild(checkbox);

                            label.innerHTML = allbranchesdata[i].Name;
                            //                             document.getElementById('ul' + plants[p]).appendChild(label);
                            //                             document.getElementById('ul' + plants[p]).appendChild(hidden);
                            //                             document.getElementById('ul' + plants[p]).appendChild(document.createElement("br"));
                            document.getElementById(allbranchesdata[i].Name).appendChild(label);
                            document.getElementById(allbranchesdata[i].Name).appendChild(hidden);
                            document.getElementById(allbranchesdata[i].Name).appendChild(document.createElement("br"));

                        }
                    }
                }
            }




            //             var nodes2 = document.getElementById("divchblroutes").getElementsByTagName('*');
            //             for (var j = 0; j < nodes2.length; j++) {
            //                 nodes2[j].disabled = true;
            //             }
        }


        function minusclick(thisid) {
            var prntdiv = $(thisid).parents(".titledivcls");
            ul = prntdiv.next("ul");
            if (thisid.title == "Hide") {
                ul.slideUp("slow");
                $(thisid).attr('title', "Show");
                $(thisid).css("background", "url('Images/plus.png') no-repeat");
            }
            else {
                ul.slideDown("slow");
                $(thisid).attr('title', "Hide");
                $(thisid).css("background", "url('Images/minus.png') no-repeat");
            }
        }



        function openPopupin(inid) {

            document.getElementById('fieldpopup').style.display = 'block';
            document.getElementById('spn_btnid').innerHTML = inid;
            document.getElementById('spn_alrtheading').innerHTML = "InPOI";
            var inpoibtn = document.getElementById('btn_inpoiedit').value;
            if (inpoibtn == "ADD") {
                $('.chkclass').each(function () {
                    $(this).attr("checked", false);
                });
            }
            else {


                var inpoidiv = document.getElementById('inlocids').innerHTML;
                $('.chkclass').each(function () {
                    $(this).attr("checked", false);
                });
                var Branchesarr = [];
                Branchesarr = inpoidiv.split("<br>");
                $('#divchblroutes').css('display', 'block');
                if (Branchesarr.length != 0) {
                   
                    for (var i = 0; i < Branchesarr.length; i++) {
                        $('.chkclass').each(function () {
                            if ($(this).next().next().val() == Branchesarr[i]) {
                                $(this).attr("checked", true);
                            }
                        });
                    }
                }
            }
        }

        function openPopupout(outid) {

            document.getElementById('fieldpopup').style.display = 'block';
            document.getElementById('spn_btnid').innerHTML = outid;
            document.getElementById('spn_alrtheading').innerHTML = "OutPOI";

            var outpoibtn = document.getElementById('btn_outpoiedit').value;
            if (outpoibtn == "ADD") {
                $('.chkclass').each(function () {
                    $(this).attr("checked", false);
                });
                
            }
            else {
                var inpoidiv = document.getElementById('outlocids').innerHTML;
                $('.chkclass').each(function () {
                    $(this).attr("checked", false);
                });
                var Branchesarr = [];
                Branchesarr = inpoidiv.split("<br>");
                $('#divchblroutes').css('display', 'block');
                if (Branchesarr.length != 0) {
                    
                    for (var i = 0; i < Branchesarr.length; i++) {
                        $('.chkclass').each(function () {
                            if ($(this).next().next().val() == Branchesarr[i]) {
                                $(this).attr("checked", true);
                            }
                        });
                    }
                }
            }

        }
        function openPopupstp(stpid) {



            document.getElementById('fieldpopup').style.display = 'block';
            document.getElementById('spn_btnid').innerHTML = stpid;
            document.getElementById('spn_alrtheading').innerHTML = "Stoppage";
            var stopmaxinpoitime = document.getElementById('txt_stpinpoi').value;
            var stoppagebtn = document.getElementById('btn_inpostp').value;
            if (stoppagebtn == "ADD") {
                if (stopmaxinpoitime == "") {
                    alert("Please Fill MAX Stop Time");
                    document.getElementById('fieldpopup').style.display = 'none';
                    return false;
                }
                else {
                    $('.tblelocmaxtm').each(function () {
                        var test2 = $(this).html();
                        if (test2 == stopmaxinpoitime) {
                            alert("You have Aleready Selected this time Enter another time");
                            document.getElementById('txt_stpinpoi').value = "";
                            document.getElementById('fieldpopup').style.display = 'none';
                        }
                        else {

                        }
                    });

                    $('.chkclass').each(function () {
                        $(this).attr("checked", false);
                    });
                    
                }
            }
            else {

            }


        }

        function btn_outpostpclick(noalrtid) {
            //document.getElementById('txt_stpoutpoi').disabled = true;
            document.getElementById('fieldpopup').style.display = 'block';
            document.getElementById('spn_btnid').innerHTML = noalrtid;
            document.getElementById('spn_alrtheading').innerHTML = "Exclude Alerts";
            var btn_outpostp = document.getElementById('btn_outpostp').value;

            if (btn_outpostp == "ADD Exclude Locations") {
                $('.chkclass').each(function () {
                    $(this).attr("checked", false);
                });
               
            }
            else {
                var noalertdiv = document.getElementById('tablenoalertdivids').innerHTML;
                $('.chkclass').each(function () {
                    $(this).attr("checked", false);
                });
                var Branchesarr = [];
                Branchesarr = noalertdiv.split("<br>");
                if (Branchesarr[0] != "") {
                    $('#divchblroutes').css('display', 'block');
                    for (var i = 0; i < Branchesarr.length; i++) {
                        $('.chkclass').each(function () {
                            //                             if ($(this).next().text() == Branchesarr[i]) {
                            //                                 $(this).attr("checked", true);
                            //                             }
                            if ($(this).next().next().val() == Branchesarr[i]) {
                                $(this).attr("checked", true);
                            }
                        });
                    }
                }
            }
        }

        function closePopup() {
            document.getElementById('fieldpopup').style.display = 'none';
            document.getElementById('btn_inpostp').value = "ADD";
            document.getElementById('txt_stpinpoi').value = "";
            $(tablerowid).css('background-color', 'rgb(243, 229, 229)');
        }

        ////------------------>for addingLocations<-----------------------//

        var tablerowid = "";
        function for_addinglocations() {
            var LocationNames = $("#divchblroutes input:checkbox:checked").map(function () {
                return $(this).next().text();
            }).toArray();
            var LocationIDs = $("#divchblroutes input:checkbox:checked").map(function () {
                return $(this).next().next().val();
            }).toArray();
            var divslct = document.getElementById('spn_btnid').innerHTML;
            if (divslct == "btn_inpoiedit") {
                document.getElementById('inlocations').innerHTML = "";
                document.getElementById('inlocids').innerHTML = "";
                for (var i = 0; i < LocationNames.length; i++) {
                    document.getElementById('inlocations').innerHTML += i + 1 + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + LocationNames[i] + "<br />";
                }
                for (var l = 0; l < LocationIDs.length; l++) {
                    document.getElementById('inlocids').innerHTML += LocationIDs[l] + "<br />";

                }
                document.getElementById('btn_inpoiedit').value = "EDIT";
            }
            else if (divslct == "btn_outpoiedit") {
                document.getElementById('outlocations').innerHTML = "";
                document.getElementById('outlocids').innerHTML = "";
                for (var j = 0; j < LocationNames.length; j++) {
                    document.getElementById('outlocations').innerHTML += j + 1 + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + LocationNames[j] + "<br />";

                }
                for (var m = 0; m < LocationIDs.length; m++) {
                    document.getElementById('outlocids').innerHTML += LocationIDs[m] + "<br />";

                }
                document.getElementById('btn_outpoiedit').value = "EDIT";

            }
            else if (divslct == "btn_inpostp") {
                var stpinpoistptme = document.getElementById('txt_stpinpoi').value;
                var locations = "";
                var ids = "";
                var alrdybrnches = "";
                var inpoistpbtn = document.getElementById('btn_inpostp').value;
                if (tablerowid != "") {
                    $(tablerowid).remove();
                }
                $('.tbleloc').each(function () {
                    var test = $(this).html();
                    var Branchesarr2 = [];
                    Branchesarr2 = test.split("<br>");
                    for (var i = 0; i < LocationNames.length; i++) {
                        for (j = 0; j < Branchesarr2.length; j++) {
                            if (Branchesarr2[j] == LocationNames[i]) {
                                alrdybrnches += LocationNames[i] + ";";
                            }
                        }
                    }
                });
                // alert(alrdybrnches);

                //                 var names = ["Mike", "Matt", "Nancy", "Adam", "Jenny", "Nancy", "Carl"];
                //                 var uniqueNames = [];
                //                 $.each(names, function (i, el) {
                //                     if ($.inArray(el, uniqueNames) === -1) uniqueNames.push(el);
                //                 });
                if (alrdybrnches == "") {


                    for (var k = 0; k < LocationNames.length; k++) {
                        locations += LocationNames[k] + "<br />";
                    }
                    for (var n = 0; n < LocationIDs.length; n++) {
                        ids += LocationIDs[n] + "<br />";
                    }
                    locations = locations.substring(0, locations.length - 6);
                    ids = ids.substring(0, ids.length - 6);
                    $('#tablestpinpoishow tr:last').after('<tr onclick="gettablerow(this)" class="tbleclassstp"><td class="tblelocmaxtm">' + stpinpoistptme + '</td><td height=50px class="tbleclaslocrow"><div class="tbleloc" style="width: 100%;height: 100%;margin: 0;padding: 0;overflow: auto;">' + locations + '</div></td><td class="tbleids" height=50px hidden="hidden"><div class="tableidsdiv" style="width: 100%;height: 100%;margin: 0;padding: 0;overflow: auto;">' + ids + '</div></td></tr>');

                    document.getElementById('txt_stpinpoi').value = "";

                    document.getElementById('btn_inpostp').value = "ADD";
                }
                else {
                    alert("These locations are already assigned to other stoppage time /n" + alrdybrnches);
                    document.getElementById('fieldpopup').style.display = 'block';
                    document.getElementById('spn_btnid').innerHTML = stpid;
                    document.getElementById('spn_alrtheading').innerHTML = "Stoppage";
                    var stopmaxinpoitime = document.getElementById('txt_stpinpoi').value;
                    var stoppagebtn = document.getElementById('btn_inpostp').value;

                    $('.chkclass').each(function () {
                        $(this).attr("checked", false);
                    });
                    if (LocationNames[0] != "") {
                        $('#divchblroutes').css('display', 'block');
                        for (var i = 0; i < LocationNames.length; i++) {
                            $('.chkclass').each(function () {
                                if ($(this).next().text() == LocationNames[i]) {
                                    $(this).attr("checked", true);
                                }
                            });
                        }
                    }
                }

            }
            else if (divslct == "btn_outpostp") {
                document.getElementById('tablenoalertdiv').innerHTML = "";
                document.getElementById('tablenoalertdivids').innerHTML = "";
                for (var j = 0; j < LocationNames.length; j++) {
                    document.getElementById('tablenoalertdiv').innerHTML += j + 1 + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + LocationNames[j] + "<br />";

                }
                for (var m = 0; m < LocationIDs.length; m++) {
                    document.getElementById('tablenoalertdivids').innerHTML += LocationIDs[m] + "<br />";

                }
                document.getElementById('btn_outpostp').value = "EDIT Exclude Locations";

            }
            closePopup();
        }



        function gettablerow(thisid) {
            $(thisid).css('background-color', '#ffffff');
            var time = $(thisid).children('.tblelocmaxtm').html();
            var locs = $(thisid).children('.tbleclaslocrow').children('.tbleloc').html();
            var invlocss = [];
            invlocss = locs.split('<br>');

            document.getElementById('txt_stpinpoi').value = time;
            document.getElementById('btn_inpostp').value = "EDIT";
            var confi = confirm("Wanna Edit Selected Row Click on Edit Button");
            if (confi) {
                $('.chkclass').each(function () {
                    $(this).attr("checked", false);
                });
                if (invlocss[0] != "") {
                    $('#divchblroutes').css('display', 'block');
                    for (var i = 0; i < invlocss.length; i++) {
                        $('.chkclass').each(function () {
                            if ($(this).next().text() == invlocss[i]) {
                                $(this).attr("checked", true);
                            }
                        });
                    }
                }
            }
            else {
                document.getElementById('txt_stpinpoi').value = "";
                document.getElementById('btn_inpostp').value = "ADD";
                $(thisid).css('background-color', 'rgb(243, 229, 229)');
            }
            tablerowid = thisid;

        }
         
     </script>

     <script type="text/javascript">
         //////----------------------->For Database Functions<---------------------------//////

         function for_main_save() {
             var sno = document.getElementById('alrt_sno').innerHTML;
             var subsno = document.getElementById('sub_sno').innerHTML;

             var alertname = document.getElementById('txt_alrtassnnme').value;
             var alerttype = [];

             var obje = { 'speedchk': speedchk, 'inpoichk': inpoichk, 'outpoichk': outpoichk, 'stoppagechk': stoppagechk, 'stpinpoichk': stpinpoichk, 'stpoutpoichk': stpoutpoichk, 'mainpowerchk': mainpowerchk }
             alerttype.push(obje);

             var t = document.getElementById('slct_alrtrepeat');
             var alrtrept = t.options[t.selectedIndex].text;
             var timegap = document.getElementById('txt_timegap').value;
             var maxspeed = document.getElementById('txt_speed').value;
             //             var inp = document.getElementById('inlocids').innerHTML;
             //             var outp = document.getElementById('outlocids').innerHTML;
             //var inp =$('#inlocids').html();
             //var outp = $('#outlocids').html();
             //             var inp = $('#inlocids');
             //inp.html($.trim(inp.html());
             var $mydiv = $('#inlocids');
             $mydiv.html($.trim($mydiv.html()));
             var $mydiv2 = $('#outlocids');
             $mydiv2.html($.trim($mydiv2.html()));
             var $mydiv3 = $('#tablenoalertdivids');
             $mydiv3.html($.trim($mydiv3.html()));

             var inp = $mydiv.html();
             var inp = inp.substring(0, inp.length - 4);
             var outp = $mydiv2.html();
             var outp = outp.substring(0, outp.length - 4);
             var noalt = $mydiv3.html();
             var noalt = noalt.substring(0, noalt.length - 4);
             var inpoi = [];
             var outpoi = [];
             var noalert = [];
             inpoi = inp.split('<br>');
             outpoi = outp.split('<br>');
             noalert = noalt.split('<br>');

             var tablelocs = [];
             $('#tablestpinpoishow tr:not(:has(th))').each(function () {

                 var logstime = $(this).children('.tblelocmaxtm').html();

                 var logsid = $(this).children('.tbleids').children('.tableidsdiv').html();
                 var logsids = logsid.split('<br>');
                 var obj = { 'maxtime': logstime, 'loc_logs': logsids }
                 tablelocs.push(obj);

             });

             var mail;
             var mobile;
             if (document.getElementById('chk_email').checked == true) {
                 mail = 1;
             }
             else {
                 mail = 0;
             }
             if (document.getElementById('chk_mobile').checked == true) {
                 mobile = 1;
             }
             else {
                 mobile = 0;
             }
             var stpmaxstptme = document.getElementById('txt_stpoutpoi').value;
             var btnval = document.getElementById('btn_mainsave').value;
             var confi;
             if (btnval == "SAVE") {
                 confi = confirm("Do you want to SAVE This Alert Group????");
             }
             else {
                 confi = confirm("Do you want to EDIT This Alert Group????");
             }
             if (confi) {
                 var Data = { 'op': 'Vehicle_alrts_save', 'alertname': alertname, 'alerttype': alerttype, 'alrtrept': alrtrept, 'timegap': timegap, 'maxspeed': maxspeed, 'inpoi': inpoi,
                     'outpoi': outpoi, 'tablelocs': tablelocs, 'stpmaxstptme': stpmaxstptme, 'btnval': btnval, 'mail': mail, 'mobile': mobile, 'sno': sno, 'subsno': subsno, 'noalert': noalert
                 };
                 var s = function (msg) {
                     if (msg) {
                         if (msg.length > 0) {
                             alert(msg);
                             forclearall();
                             retrievealrtnme();
                             document.getElementById('sub_sno').innerHTML = "";
                             document.getElementById('alrt_sno').innerHTML = "";
                            
                            
                         }
                     }
                     else {
                     }
                 };
                 var e = function (x, h, e) {
                 };
                 CallHandlerUsingJson(Data, s, e);
             }
         }


         function retrievealrtnme() {
             var data = { 'op': 'retrieve_alrtname' };
             var s = function (msg) {
                 if (msg) {
                     if (msg.length > 0) {
                         var alerts = "";
                         for (var i = 0; i < msg.length; i++) {
                             alerts += "<a onclick='getalert_data(this)' data-value='" + msg[i].alert_sno + "'>" + msg[i].alert_name + "</a><br />";
                         }
                         document.getElementById('alertgroups').innerHTML = alerts;

                     }
                 }
                 else {
                 }
             };
             var e = function (x, h, e) {
             };
             $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

             callHandler(data, s, e);
         }

         function getalert_data(thisid) {
             var alertname = $(thisid).html();
             var alertsno = $(thisid).attr('data-value');
             var data = { 'op': 'rettrieve_alert_data', 'alertname': alertname, 'alertsno': alertsno };
             var s = function (msg) {
                 if (msg) {
                     if (msg.length > 0) {
                         filldata(msg);
                     }
                 }
                 else {
                 }
             };
             var e = function (x, h, e) {
             };
             $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

             callHandler(data, s, e);
         }
         var subsno = [];
         var sbsno = "";
         function filldata(msg) {
             $('#tablestpinpoishow tbody > tr:not(:first)').remove();

             document.getElementById('alrt_sno').innerHTML = msg[0].sno;
             document.getElementById('txt_alrtassnnme').value = msg[0].alert_nam;
             document.getElementById('slct_alrtrepeat').value = msg[0].nooftimes;
             document.getElementById('txt_timegap').value = msg[0].timegap;
             if (msg[0].email == "1") {
                 document.getElementById('chk_email').checked = true;
             }
             if (msg[0].mobile == "1") {
                 document.getElementById('chk_mobile').checked = true;
             }
             document.getElementById('txt_speed').value = msg[0].nooftimes;

             var alert_type = msg[0].alert_type;
             for (var i = 0; i < alert_type.length; i++) {
                 if (alert_type[i].alert_type == "mainpower") {
                     document.getElementById('chk_mainpower').checked = true;
                 }
                 if (alert_type[i].alert_type == "speed") {
                     document.getElementById('chk_speedenable').checked = true;
                     document.getElementById('txt_speed').value = alert_type[i].value;
                 }
                 if (alert_type[i].alert_type == "inpoi") {
                     document.getElementById('chk_inpoienable').checked = true;
                     var locs = alert_type[i].loc_id;
                     var inpoilocs = "";
                     var inpoiids = "";
                     for (var j = 0; j < locs.length; j++) {
                         inpoilocs += j + 1 + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + locs[j].loc_name + "<br />";
                         inpoiids += locs[j].loc_id + "<br />";
                     }
                     document.getElementById('inlocations').innerHTML = inpoilocs;
                     document.getElementById('inlocids').innerHTML = inpoiids;
                     document.getElementById('btn_inpoiedit').value = "EDIT";
                 }
                 if (alert_type[i].alert_type == "outpoi") {
                     document.getElementById('chk_outpoienable').checked = true;
                     var locs = alert_type[i].loc_id;
                     var outpoilocs = "";
                     var outpoiids = "";
                     for (var j = 0; j < locs.length; j++) {
                         outpoilocs += j + 1 + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + locs[j].loc_name + "<br />";
                         outpoiids += locs[j].loc_id + "<br />";
                     }
                     document.getElementById('outlocations').innerHTML = outpoilocs;
                     document.getElementById('outlocids').innerHTML = outpoiids;
                     document.getElementById('btn_outpoiedit').value = "EDIT";

                 }
                 if (alert_type[i].alert_type == "stoppage") {
                     document.getElementById('chk_stoppageenable').checked = true;
                     if (alert_type[i].sub_type == "inpoi") {
                         document.getElementById('chk_stpinpoi').checked = true;

                         var locs = alert_type[i].loc_id;
                         var stpinpoilocs = "";
                         var stpinpoiids = "";
                         var prevno = "";
                         for (var j = 0; j < locs.length; j++) {
                             if (prevno == "")
                                 prevno = locs[j].locval;

                             if (prevno == locs[j].locval) {
                                 stpinpoilocs += locs[j].loc_name + "<br />";
                                 stpinpoiids += locs[j].loc_id + "<br />";

                             }
                             else {
                                 stpinpoilocs = stpinpoilocs.substring(0, stpinpoilocs.length - 6);
                                 stpinpoiids = stpinpoiids.substring(0, stpinpoiids.length - 6)
                                 $('#tablestpinpoishow tr:last').after('<tr onclick="gettablerow(this)" class="tbleclassstp"><td class="tblelocmaxtm">' + prevno + '</td><td height=50px class="tbleclaslocrow"><div class="tbleloc" style="width: 100%;height: 100%;margin: 0;padding: 0;overflow: auto;">' + stpinpoilocs + '</div></td><td class="tbleids" height=50px hidden="hidden"><div class="tableidsdiv" style="width: 100%;height: 100%;margin: 0;padding: 0;overflow: auto;">' + stpinpoiids + '</div></td></tr>');
                                 prevno = locs[j].locval;
                                 stpinpoilocs = "";
                                 stpinpoiids = "";
                                 stpinpoilocs += locs[j].loc_name + "<br />";
                                 stpinpoiids += locs[j].loc_id + "<br />";
                             }
                         }

                         $('#tablestpinpoishow tr:last').after('<tr onclick="gettablerow(this)" class="tbleclassstp"><td class="tblelocmaxtm">' + prevno + '</td><td height=50px class="tbleclaslocrow"><div class="tbleloc" style="width: 100%;height: 100%;margin: 0;padding: 0;overflow: auto;">' + stpinpoilocs + '</div></td><td class="tbleids" height=50px hidden="hidden"><div class="tableidsdiv" style="width: 100%;height: 100%;margin: 0;padding: 0;overflow: auto;">' + stpinpoiids + '</div></td></tr>');



                     }
                     if (alert_type[i].sub_type == "outpoi") {
                         document.getElementById('chk_stpoutpoi').checked = true;
                         document.getElementById('tablenoalertdiv').innerHTML = "";
                         document.getElementById('tablenoalertdivids').innerHTML = "";
                         document.getElementById('txt_stpoutpoi').value = alert_type[i].value;
                         var locs = alert_type[i].loc_id;
                         var stpnoalrtlocs = "";
                         var stpnoalrtlocsids = "";
                         for (var j = 0; j < locs.length; j++) {
                             stpnoalrtlocs += j + 1 + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + locs[j].loc_name + "<br />";
                             stpnoalrtlocsids += locs[j].loc_id + "<br />";
                         }
                         document.getElementById('tablenoalertdiv').innerHTML = stpnoalrtlocs;
                         document.getElementById('tablenoalertdivids').innerHTML = stpnoalrtlocsids;
                         document.getElementById('btn_outpostp').value = "EDIT Exclude Locations";

                     }
                 }
             }
             forspeeddiv();
             forinpoidiv();
             foroutpoidiv();
             forstoppagediv();
             forstopinpoi();
             forstopoutpoi();
             document.getElementById('btn_mainsave').value = "EDIT";
             if (msg[0].status == "1") {
                 document.getElementById('btn_status').value = "DisAble";
             }

             subsno = msg[0].subsno;

             for (var k = 0; k < subsno.length; k++) {
                 sbsno += subsno[k].subsno + ",";
             }
             sbsno = sbsno.substring(0, sbsno.length - 1);
             document.getElementById('sub_sno').innerHTML = sbsno;
         }


         function for_alert_delete() {
             var subs = document.getElementById('sub_sno').innerHTML;
             var sno = document.getElementById('alrt_sno').innerHTML;
             var data = { 'op': 'for_alert_delete', 'subs': subs, 'sno': sno };
             var s = function (msg) {
                 if (msg) {
                     if (msg.length > 0) {
                         alert(msg);
                         forclearall();
                         retrievealrtnme();
                     }
                 }
                 else {
                 }
             };
             var e = function (x, h, e) {
             };
             $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

             callHandler(data, s, e);
         }

         function for_disable() {
             var sno = document.getElementById('alrt_sno').innerHTML;
             var status = document.getElementById('btn_status').value;
             var confi = confirm("Do you want to Change status of this Alert????");
             if (confi) {
                 var data = { 'op': 'for_alert_status', 'status': status, 'sno': sno };
                 var s = function (msg) {
                     if (msg) {
                         if (msg.length > 0) {
                             alert(msg);
                             //forclearall();
                             if (status == "Enable") {
                                 document.getElementById('btn_status').value = "DisAble";
                             }
                             else {
                                 document.getElementById('btn_status').value = "Enable";
                             }

                         }
                     }
                     else {
                     }
                 };
                 var e = function (x, h, e) {
                 };
                 $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

                 callHandler(data, s, e);
             }
         }

         function forclearall() {
             var speedchk = "";
             var inpoichk = "";
             var outpoichk = "";
             var stoppagechk = "";
             var stpinpoichk = "";
             var stpoutpoichk = "";
             var mainpowerchk = "";
             document.getElementById('txt_alrtassnnme').value = "";
             document.getElementById('slct_alrtrepeat').value = "0";
             document.getElementById('txt_timegap').value = "";
             document.getElementById('chk_email').checked = false;
             document.getElementById('chk_mobile').checked = false;
             document.getElementById('chk_speedenable').checked = false;
             document.getElementById('chk_inpoienable').checked = false;
             document.getElementById('chk_outpoienable').checked = false;
             document.getElementById('chk_stoppageenable').checked = false;
             document.getElementById('chk_stpinpoi').checked = false;
             document.getElementById('chk_stpoutpoi').checked = false;
             document.getElementById('txt_speed').value = "";
             document.getElementById('inlocations').innerHTML = "";
             document.getElementById('inlocids').innerHTML = "";
             document.getElementById('outlocations').innerHTML = "";
             document.getElementById('outlocids').innerHTML = "";
             document.getElementById('tablenoalertdiv').innerHTML = "";
             document.getElementById('tablenoalertdivids').innerHTML = "";
             document.getElementById('txt_stpoutpoi').value = "";
             document.getElementById('chk_mainpower').checked = false;
             
             //  $('tablestpinpoishow').children('tr:not(:first)').remove();
             // $('tablestpinpoishow tr:not(:first)').remove();
             //$('#tablestpinpoishow tbody > tr').remove();
             $('#tablestpinpoishow tbody > tr:not(:first)').remove();
             document.getElementById('btn_mainsave').value = "SAVE";
             document.getElementById('btn_delete').value = "DELETE";
             document.getElementById('btn_status').value = "Enable";
             document.getElementById('btn_inpoiedit').value = "ADD";
             document.getElementById('btn_outpoiedit').value = "ADD";
             forspeeddiv();
             forinpoidiv();
             foroutpoidiv();
             forstoppagediv();
             forstopinpoi();
             forstopoutpoi();
         }

         function removetblerow() {
             var confi = confirm("Do You Want to Remove Selected Row");
             if (confi) {
                 $(tablerowid).remove();
             }
         }


     </script>


    
    <div align="center">
        <table>
            <tr>
                <td>
                    Alert Name
                </td>
                <td>
                    <input id="txt_alrtassnnme" type="text" />
                </td>
                <td>
                <span hidden="hidden" id="alrt_sno"></span>
                </td>
                <td>
                <span hidden="hidden" id="sub_sno"></span>
                </td>
            </tr>
            <tr>
                <td>
                    Alert Repeat
                </td>
                <td>
                    <select id="slct_alrtrepeat" style="width: 115px;">
                        <option>0</option>
                        <option>1</option>
                        <option>2</option>
                        <option>3</option>
                    </select>Times
                </td>
                <td>
                    With Timegap Of
                </td>
                <td>
                    <input id="txt_timegap" type="text" />
                    &nbsp;&nbsp;In Min
                </td>
            </tr>
            <tr>
            <td colspan="2">
            Send alerts To
            </td>
            <td>
                <input id="chk_email" type="checkbox" />&nbsp;Email
            </td>
            <td>
                <input id="chk_mobile" type="checkbox" />&nbsp;Mobile
            </td>
            </tr>
            <tr>
            <td colspan="2">
            Main Power Lost Alert
            </td>
            <td>
                <input id="chk_mainpower" type="checkbox" onchange="formainalertchk()"/>&nbsp;Check here 
            </td>
            </tr>

        </table>
    </div>
    
    <div align="center" style="width: 100%; height: 100%;">
        <div id="ldiv">
         <div id="alertgroups" style="float: left; width: 200px;height:530px; border: 1px solid gray;overflow:auto;">
                        </div>
        </div>
        <div id="rdiv">
            <%--Speed Div--%>

            <div class="headerDiv">
            <div class="titleText">SPEED</div>
            <div class="checkbox" style="margin-left: 75px;"><input id="chk_speedenable" type="checkbox" onchange="forspeeddiv()" />&nbsp;&nbsp;Check the box to Enable
            </div>
            </div>
            <div class="follwingDiv" align="Center" id="speeddiv" style="height:50px;">
                <table>
                    <tr>
                        <td>
                            Maximum Speed
                        </td>
                        <td>
                            <input id="txt_speed" type="text" /> &nbsp;&nbsp;In Km/Hr
                        </td>
                    </tr>
                </table>
            </div>
            <div style="float:right;">
                 <input type="button" id="btn_inpoiedit" value="ADD" class="btnstyle" onclick="openPopupin(this.id)"/>
                </div>
            <%--InPOI division--%>
            <div class="headerDiv">
            <div class="titleText">InPOI</div>
            <div class="checkbox" style="margin-left: 110px;"><input id="chk_inpoienable" type="checkbox" onchange="forinpoidiv()" />&nbsp;&nbsp;Check the box to Enable
            </div>
            </div>
            <div class="follwingDiv" align="Center" id="inpoidiv">
                <div id="inlocations" style="overflow:auto;margin-left:100px;" align="left">
                
                </div>
                <div id="inlocids" hidden="hidden">
                
                </div>
                
            </div>
            <div style="float:right;">
                 <input type="button" id="btn_outpoiedit" value="ADD" class="btnstyle" onclick="openPopupout(this.id)"/>
                </div>

            <%--Out POI division--%>

            <div class="headerDiv">
            <div class="titleText">OutPOI</div>
            <div class="checkbox" style="margin-left: 110px;"><input id="chk_outpoienable" type="checkbox" onchange="foroutpoidiv()" />&nbsp;&nbsp;Check the box to Enable
            </div>
            </div>
            <div class="follwingDiv" align="Center" id="outpoidiv">
                <div id="outlocations" style="overflow:auto;margin-left:100px;" align="left">
                
                </div>
                <div id="outlocids" hidden="hidden">
                
                </div>
                
            </div>

            <%--Stoppage Division--%>

            <div class="headerDiv">
            <div class="titleText">STOPPAGE</div>
            <div class="checkbox"><input id="chk_stoppageenable" type="checkbox" onchange="forstoppagediv()"/>&nbsp;&nbsp;Check the box to Enable
            </div>
            </div>
            <div class="follwingDiv" align="Center" id="stoppagediv" style="height:350px;">
                <div id="stoppagegrid">
                    <%--<table id="grid_stoppage"></table>
                    <div id="pager"></div>--%>
                    <br />
                    <br />
                    <div style="width: 90%;"align="center">
                        <div class="SubheaderDiv">
                            <div class="subtitleText">
                                Inpoi</div>
                                <div class="subcheckbox" style="margin-left: 64px;"><input id="chk_stpinpoi" type="checkbox" onchange="forstopinpoi()"/>&nbsp;&nbsp;Check the box to Enable
            </div>
                        </div>
                        <div id="stopinpoidiv">
                            <table>
                                <tr>
                                    <td>
                                        Max Stop Time
                                    </td>
                                    <td>
                                        <input id="txt_stpinpoi" type="text" /> &nbsp;&nbsp;In Min
                                    </td>
                                   <%-- <td>
                                    Locations
                                </td>
                                <td>
                                    <input id="txt_stoplocations" type="text" />
                                </td>--%>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <input id="btn_inpostp" type="button" value="ADD" class="btnstyle" onclick="openPopupstp(this.id)"/>
                                        <input id="btn_removeinpostp" type="button" value="Remove Row" class="btnstyle" onclick="removetblerow()"/>
                                    </td>
                                </tr>
                            </table>
                            <div style="overflow:auto;height:130px;">
                                <table id="tablestpinpoishow" style="width:65%;" >
                                    <tr>
                                        <th>
                                            Max Stop Time
                                        </th>
                                        <th>
                                            Locations
                                        </th>
                                        <th hidden="hidden">
                                        IDS
                                        </th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        
                    </div>
                    
                    <div style="width:90%;" align="center">
                    <div class="SubheaderDiv">
                     <div class="subtitleText">OutPOI</div>
                     <div class="subcheckbox"><input id="chk_stpoutpoi" type="checkbox" onchange="forstopoutpoi()"/>&nbsp;&nbsp;Check the box to Enable
            </div>
                    </div>

                    <div id="stopoutpoidiv">
                    
                        <table>
                            <tr>
                                <td>
                                    Max Stop Time
                                </td>
                                <td>
                                    <input id="txt_stpoutpoi" type="text" />&nbsp;&nbsp;In Min
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10" align="center">
                                    <input id="btn_outpostp" type="button" value="ADD Exclude Locations" class="btnstyle" onclick="btn_outpostpclick(this.id)"/>
                                </td>
                            </tr>
                        </table>

                    <div id="tablenoalertdiv" style="overflow:auto;margin-left:240px;" align="left">
                    
                    </div>
                    <div id="tablenoalertdivids" style="display:none;">
                    
                    </div>
                    </div>

                    </div>
                    
                    <%--<div style="width: 90%;" align="center">
                        <div class="SubheaderDiv">
                            <div class="subtitleText">
                                No Alert Zone</div>
                            <div class="subcheckbox" style="margin-right: 44px;">
                                <input id="chk_nolaert" type="checkbox" onchange="forstopnoalert()" />&nbsp;&nbsp;Check
                                the box to Enable
                            </div>
                        </div>
                    </div>--%>

                    <%--<div id="stopnoalertdiv">
                    
                    </div>--%>
                    
                </div>
                
                


                <%--<div style="float:right;">
                 <input type="button" id="btn_stoppsgeedit" value="ADD" class="btnstyle" onclick="openPopupstp(this.id)"/>
                </div>--%>
            </div>
             <input id="btn_mainsave" type="button" value="SAVE" class="btn" onclick="for_main_save()"/>
        <input id="btn_delete" type="button" value="DELETE" class="btn" onclick="for_alert_delete()"/>
        <input id="btn_status" type="button" value="Enable" class="btn" onclick="for_disable()"/>
        <input id="btn_clear" type="button" value="Clear" class="btn" onclick="forclearall()"/>

            
        </div>
        
        <%--<div id="btndiv">--%>
       
        <%--</div>--%>

    </div>

    <div id="fieldpopup" class="pickupclass" style="text-align: center; height: 100%;
        width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
        background: rgba(192, 192, 192, 0.7);">
        <div id="fieldslct" style="border: 5px solid #A0A0A0; position: absolute; top: 1%;
            overflow: auto; background-color: White; left: 15%; right: 15%; width: 75%; height: 75%;
            -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
            box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); border-radius: 10px 10px 10px 10px;">
            <br />
            <br />
       <%--     <div style="width: 100%; overflow: auto; margin-left: 0px; border-radius: 10px;"
                align="center">--%>
                <div id="searchtextdiv">
                <label>Search Location</label>&nbsp;&nbsp;&nbsp;&nbsp;<input type="text" id="txt_search" />
                </div>
                <div id="divchblroutes" style="width: 50%;float:left; height: 90%; border: 1px solid gray;overflow:auto;">
                </div>
                <div id="div_alrtheading" style="margin-top:10%;">
                <span id="spn_alrtheading" style="font-size:30px;"></span><span id="spn_btnid" hidden="hidden"></span>
                </div>
                <div style="float:left;width: 20%;margin-top: 10%;" align="center">
                    <input id="btn_locbtnadd" type="button" value="ADD Locations" class="btnstyle" onclick="for_addinglocations()"/>
                    <input id="btn_cancel" type="button" value="Cancel" class="btnstyle" onclick="closePopup()" />
                </div>
        <%--    </div>--%>
            <div>
            </div>
            <div id="divclose" style="width: 25px; top: 0%; right: 0%; position: absolute; z-index: 99999;
                cursor: pointer;">
                <img src="Images/PopClose.png" alt="close" onclick="closePopup();" />
            </div>
        </div>
    </div>




</asp:Content>

