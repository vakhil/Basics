<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmployeeMaster.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="EmployeeMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <style type="text/css">
        .txtClass
        {
            width: 280px;
            height: 30px;
            font-size: 14px;
            border: 1px solid gray;
            border-radius: 7px 7px 7px 7px;
        }
        .txtClassforDate
        {
            width: 165px;
            height: 30px;
            font-size: 14px;
            border: 1px solid gray;
            border-radius: 7px 7px 7px 7px;
        }
        .requiredcls
        {
            color:Red;
            font-size:20px;
            font-weight:bold;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            UpadteGrid();
        });
        function BtnSaveClick() {
            var txtEmployeeID = document.getElementById('txtEmployeeID').value;
            var txtEmployeeName = document.getElementById('txtEmployeeName').value;
            var txtAddress = document.getElementById('txtAddress').value;
            var txtPhoneNo = document.getElementById('txtPhoneNo').value;
            var txtPAddress = document.getElementById('txtPAddress').value;
            var txtPPhoneNo = document.getElementById('txtPPhoneNo').value;
            var txtDOB = document.getElementById('txtDOB').value;
            var txtLicenceNo = document.getElementById('txtLicenceNo').value;
            var txtBloodGroup = document.getElementById('txtBloodGroup').value;
            var txtINExpiryDate = document.getElementById('txtINExpiryDate').value;
            var txtQualification = document.getElementById('txtQualification').value;
//            var txtBadgeNo = document.getElementById('txtBadgeNo').value;
//            var txtRCExpiryDate = document.getElementById('txtRCExpiryDate').value;
            var txtExperience = document.getElementById('txtExperience').value;
            var txtDOJ = document.getElementById('txtDOJ').value;
            var txtRemarks = document.getElementById('txtRemarks').value;
            var btnSave = document.getElementById('BtnSave').value; 
            var emptype = document.getElementById('ddlemptype').value;
            var empstatus = document.getElementById('ddlempstatus').value;
            if (txtEmployeeID == "" || txtEmployeeName == "" || txtAddress == "" || txtPhoneNo == "" || txtLicenceNo == "" || txtDOJ == "" || txtINExpiryDate == "" || emptype == "Select") {
                alert("Please fill required fields");
                return false;
            }
            var data = { 'op': 'EmployeeManageSaveClick', 'txtEmployeeID': txtEmployeeID, 'txtEmployeeName': txtEmployeeName, 'txtAddress': txtAddress, 'txtPhoneNo': txtPhoneNo, 'txtPAddress': txtPAddress, 'txtPPhoneNo': txtPPhoneNo, 'txtDOB': txtDOB, 'txtLicenceNo': txtLicenceNo, 'txtBloodGroup': txtBloodGroup, 'txtINExpiryDate': txtINExpiryDate, 'txtQualification': txtQualification, 'txtExperience': txtExperience, 'txtDOJ': txtDOJ, 'txtRemarks': txtRemarks, 'emptype': emptype,'empstatus':empstatus, 'btnSave': btnSave };
            var s = function (msg) {
                if (msg) {
                    document.getElementById('lblmsg').innerHTML = msg;
                    btnRefreshClick();
                    UpadteGrid();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function btnRefreshClick() {
            document.getElementById('txtEmployeeID').value = "";
            document.getElementById('txtEmployeeName').value = "";
            document.getElementById('txtAddress').value = "";
            document.getElementById('txtPhoneNo').value = "";
            document.getElementById('txtPAddress').value = "";
            document.getElementById('txtPPhoneNo').value = "";
            document.getElementById('txtDOB').value = "";
            document.getElementById('txtLicenceNo').value = "";
            document.getElementById('txtBloodGroup').value = "";
            document.getElementById('txtINExpiryDate').value = "";
            document.getElementById('txtQualification').value = "";
//            document.getElementById('txtBadgeNo').value = "";
//            document.getElementById('txtRCExpiryDate').value = "";
            document.getElementById('txtExperience').value = "";
            document.getElementById('txtDOJ').value = "";
            document.getElementById('txtRemarks').value = ""; 
            var dd = document.getElementById('ddlemptype');
            dd.selectedIndex = 0;
            dd = document.getElementById('ddlempstatus');
            dd.selectedIndex = 0;
            document.getElementById('BtnSave').value = "Save";
        }
        function btnDeleteClick() {
            var txtEmployeeID = document.getElementById('txtEmployeeID').value;
            if (txtEmployeeID == "") {
                alert("Please Select Feilds");
                return false;
            }
            var data = { 'op': 'EmployeeManamentDeleteDetails', 'txtEmployeeID': txtEmployeeID };
            var s = function (msg) {
                if (msg) {
                    document.getElementById('lblmsg').innerHTML = msg;
                    btnRefreshClick();
                    UpadteGrid();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function UpadteGrid() {
            var data = { 'op': 'UpdateEmployeeManamentDetails' };
            var s = function (msg) {
                if (msg) {
                    BindGrid(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindGrid(databind) {
            $("#Unauthorized").jqGrid("clearGridData");
            var newarray = [];
            var Headarray = [];
            var headdatacol = databind[1];
            var datacol = databind;
            for (var Booking in databind) {
                newarray.push({ 'EmployeeID': databind[Booking].EmployeeID, 'EmployeeName': databind[Booking].EmployeeName, 'EmployeeType': databind[Booking].EmployeeType, 'DoB': databind[Booking].DoB, 'Address': databind[Booking].Address, 'phoneNumber': databind[Booking].phoneNumber, 'PermanentAddress': databind[Booking].PermanentAddress, 'PermanentPhoneNo': databind[Booking].PermanentPhoneNo, 'BloodGroup': databind[Booking].BloodGroup, 'Qualification': databind[Booking].Qualification, 'Experience': databind[Booking].Experience, 'DOJ': databind[Booking].DOJ, 'LicenseNo': databind[Booking].LicenseNo, 'LicenseNoExpDT': databind[Booking].LicenseNoExpDT, 'Remarks': databind[Booking].Remarks, 'OperatorName': databind[Booking].OperatorName, 'Status': databind[Booking].Status });
            }
            for (var headdata in headdatacol) {
                Headarray.push(headdata);
            } //,,
            $("#Unauthorized").jqGrid({
                datatype: "local",
                height: 300,
                autowidth: true,
                colNames: Headarray,
                colModel: [{ name: 'EmployeeID', index: 'id', width: 100, sortable: false, align: 'center' },
                { name: 'EmployeeName', index: 'id', width: 60, sortable: false, align: 'center' },
                { name: 'EmployeeType', index: 'id', width: 60, sortable: false, align: 'center' },
{ name: 'DoB', index: 'invdate', width: 90, sortable: false, align: 'center' },
{ name: 'Address', index: 'name', width: 100, sortable: false, align: 'center' },
{ name: 'phoneNumber', index: 'note', width: 150, sortable: false, align: 'center' },
{ name: 'PermanentAddress', index: 'id', width: 100, sortable: false, align: 'center' },
{ name: 'PermanentPhoneNo', index: 'note', width: 60, sortable: false, align: 'center' },
{ name: 'BloodGroup', index: 'invdate', width: 90, sortable: false, align: 'center' },
{ name: 'Qualification', index: 'name', width: 100, sortable: false, align: 'center' },
{ name: 'Experience', index: 'note', width: 150, sortable: false, align: 'center' },
{ name: 'DOJ', index: 'invdate', width: 60, sortable: false, align: 'center' },
{ name: 'LicenseNo', index: 'id', width: 90, sortable: false, align: 'center' },
{ name: 'LicenseNoExpDT', index: 'invdate', width: 100, sortable: false, align: 'center' },
{ name: 'Remarks', index: 'id', width: 60, sortable: false, align: 'center' },
{ name: 'OperatorName', index: 'id', width: 150, sortable: false, align: 'center'},
{ name: 'Status', index: 'id', width: 150, sortable: false, align: 'center'}],
                caption: "Vehicle Mangement Details"
            });
            var mydata = newarray;
            for (var i = 0; i <= mydata.length; i++) {

                jQuery("#Unauthorized").jqGrid('addRowData', i + 1, mydata[i]);
            }
            $("#Unauthorized").jqGrid('setGridParam', {
                ondblClickRow: function (
        rowid, iRow, iCol, e) {
                    var EmployeeID = $('#Unauthorized').getCell(rowid, 'EmployeeID'); 
                    var EmployeeName = $('#Unauthorized').getCell(rowid, 'EmployeeName');
                    var EmployeeType = $('#Unauthorized').getCell(rowid, 'EmployeeType');
                    var DoB = $('#Unauthorized').getCell(rowid, 'DoB');
                    var Address = $('#Unauthorized').getCell(rowid, 'Address');
                    var phoneNumber = $('#Unauthorized').getCell(rowid, 'phoneNumber');
                    var PermanentAddress = $('#Unauthorized').getCell(rowid, 'PermanentAddress');
                    var PermanentPhoneNo = $('#Unauthorized').getCell(rowid, 'PermanentPhoneNo');
                    var BloodGroup = $('#Unauthorized').getCell(rowid, 'BloodGroup');
                    var Qualification = $('#Unauthorized').getCell(rowid, 'Qualification');
                    var Experience = $('#Unauthorized').getCell(rowid, 'Experience');
                    var DOJ = $('#Unauthorized').getCell(rowid, 'DOJ');
                    var LicenseNo = $('#Unauthorized').getCell(rowid, 'LicenseNo');
                    var LicenseNoExpDT = $('#Unauthorized').getCell(rowid, 'LicenseNoExpDT');
                    var Remarks = $('#Unauthorized').getCell(rowid, 'Remarks');
                    var OperatorName = $('#Unauthorized').getCell(rowid, 'OperatorName');
                    var empstatus = $('#Unauthorized').getCell(rowid, 'Status');
                    if (EmployeeID == false) {
                        return false;
                    }
                    document.getElementById('txtEmployeeID').value = EmployeeID;
                    document.getElementById('txtEmployeeName').value = EmployeeName;
                    document.getElementById('ddlemptype').value = EmployeeType;
                    document.getElementById('txtAddress').value = Address;
                    document.getElementById('txtPhoneNo').value = phoneNumber;
                    document.getElementById('txtPAddress').value = PermanentAddress;
                    document.getElementById('txtPPhoneNo').value = PermanentPhoneNo;
                    document.getElementById('txtDOB').value = DoB;
                    document.getElementById('txtLicenceNo').value = LicenseNo;
                    document.getElementById('txtBloodGroup').value = BloodGroup;
                    document.getElementById('txtINExpiryDate').value = LicenseNoExpDT;
                    document.getElementById('txtQualification').value = Qualification;
                    document.getElementById('txtExperience').value = Experience; 
                    document.getElementById('txtDOJ').value = DOJ;
                    document.getElementById('txtRemarks').value = Remarks;
                    document.getElementById('ddlempstatus').value = empstatus;
                    document.getElementById('BtnSave').value = "Edit";
                }
            });
        }
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
        //Function for only no
        $(document).ready(function () {
            $("#txtPhoneNo,#txtPPhoneNo,#txt_startengineworkinghrs").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 ||
                // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div style="width: 100%;">
        <div style="width: 100%;" class="HeaderClass">
            <table  align="center">
            <tr><td colspan="2" style="text-align:center;font-size: 20px;color: orange;">
                                    <span>Address</span>
            </td></tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td style="float: left;">
                                    <label for="lblVehicleID">
                                        EmployeeID</label>
                                </td>
                                <td>
                                    <input id="txtEmployeeID" type="text" placeholder="Enter EmployeeID" class="txtClass" />
                                    <span class="requiredcls">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="lblEmployeeName">
                                        EmployeeName</label>
                                </td>
                                <td>
                                    <input id="txtEmployeeName" type="text" class="txtClass" placeholder="Enter Employee Name" />
                                    <span class="requiredcls">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="lblAddress">
                                        Address</label>
                                </td>
                                <td>
                                    <textarea id="txtAddress" class="txtClass" placeholder="Enter Address"></textarea>
                                    <span class="requiredcls">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="lblPhoneNo">
                                        PhoneNo</label>
                                </td>
                                <td>
                                    <input id="txtPhoneNo" type="text" class="txtClass" placeholder="Enter PhoneNo" />
                                    <span class="requiredcls">*</span>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <span>Permenent Address</span>
                                    <label for="lblAddress">
                                        Address</label>
                                </td>
                                <td>
                                    <select id="ddlemptype" class="txtClass">
                                        <option>Select</option>
                                        <option>Driver</option>
                                        <option>Helper</option>
                                    </select>
                                    <span class="requiredcls">*</span>
                                </td>
                             </tr>
                            <tr>
                                <td>
                                    <span>Permenent Address</span>
                                    <label for="lblAddress">
                                        Address</label>
                                </td>
                                <td>
                                    <textarea id="txtPAddress" class="txtClass" placeholder="Enter Address"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="lblPhoneNo">
                                        PhoneNo</label>
                                </td>
                                <td>
                                    <input id="txtPPhoneNo" type="text" class="txtClass" placeholder="Enter PhoneNo" />
                                </td>
                            </tr>
                               <tr>
                                <td>
                                    <span>Status</span>
                                    <label for="lblAddress">
                                        Address</label>
                                </td>
                                <td>
                                    <select id="ddlempstatus" class="txtClass">
                                        <option>Active</option>
                                        <option>InActive</option>
                                    </select>
                                    <span class="requiredcls">*</span>
                                </td>
                             </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 100%;" class="HeaderClass">
            <table align="center">
             <tr><td colspan="4" style="text-align:center;font-size: 20px;color: orange;">
                                    <span>Statutory</span>
            </td></tr>
                <tr>
                    <td>
                        <label for="lblDOB">
                            DOB</label>
                    </td>
                    <td>
                        <input type="date" id="txtDOB" pattern="dd/MM/yyyy" class="txtClass" />
                    </td>
                      <td>
                        <label for="lblBloodGroup">
                            BloodGroup</label>
                    </td>
                    <td>
                        <input type="text" id="txtBloodGroup" class="txtClass" placeholder="Enter BloodGroup" />
                    </td>
                </tr>
                <tr>
                   <td>
                        <label for="lblLicenceNo">
                            LicenceNo</label>
                    </td>
                    <td>
                        <input type="text" id="txtLicenceNo" class="txtClass" placeholder="Enter LicenceNo" />
                                    <span class="requiredcls">*</span>
                    </td>
                    <td>
                        <label for="lblExpiry">
                            Expiry Date</label>
                    </td>
                    <td>
                        <input type="date" id="txtINExpiryDate" pattern="dd/MM/yyyy" class="txtClass" />
                                    <span class="requiredcls">*</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="lblQualification">
                            Qualification</label>
                    </td>
                    <td>
                        <input type="text" id="txtQualification" class="txtClass" placeholder="Enter Qualification" />
                    </td>
                    
                    <td>
                        <label for="lblExperience">
                            Experience</label>
                    </td>
                    <td>
                        <input type="text" id="txtExperience" class="txtClass" placeholder="Enter Experience" />
                    </td>
                </tr>
            <%--    <tr>
                    <td>
                        <label for="lblBadgeNo">
                            BadgeNo
                        </label>
                    </td>
                    <td>
                        <input type="text" id="txtBadgeNo" class="txtClass" placeholder="Enter BadgeNo" />
                    </td>
                    <td>
                        <label for="lblRCExpiry">
                            Expiry Date</label>
                    </td>
                    <td>
                        <input type="date" id="txtRCExpiryDate" class="txtClass" />
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        <label for="lblDOJ">
                            DOJ</label>
                    </td>
                    <td>
                        <input type="date" id="txtDOJ" class="txtClass" pattern="dd/MM/yyyy" />
                                    <span class="requiredcls">*</span>
                    </td>
                    <td>
                        <label for="lblRemarks">
                            Remarks</label>
                    </td>
                    <td>
                        <textarea id="txtRemarks" class="txtClass" placeholder="Enter Remarks"></textarea>
                    </td>
                </tr>
            </table>
            <table align="center">
                <tr>
                    <td>
                        <input type="button" id="BtnSave" value="Save" class="ContinueButton" style="width:50px;height:20px;" onclick="BtnSaveClick();" />
                    </td>
                    <td>
                        <input type="button" id="btnDelete" value="Delete" class="ContinueButton" style="width:50px;height:20px;" onclick="btnDeleteClick();" />
                    </td>
                    <td>
                        <input type="button" id="btnRefresh" value="Refresh" class="ContinueButton" style="width:50px;height:20px;" onclick="btnRefreshClick();" />
                    </td>
                </tr>
            </table>
            <table align="center">
                <tr>
                    <td>
                        <span id="lblmsg" style="color: Red;"></span>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div>
        <table id="Unauthorized" style="width: 100%;">
        </table>
    </div>
</asp:Content>
