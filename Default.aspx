<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CS_lab5._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table align="center">
        <%--Line of table name--%>
        <tr align="center">
          <td>
            <asp:Label ID="Label4" runat="server" Text="Providers list" Font-Size="20pt" ForeColor="Maroon" Font-Bold="True"/>
          </td>
        </tr>
        <%--Line for show providers--%>
          <tr>
            <td>
              <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false"
                                        ShowFooter="true" ShowHeader="true"
                                        AllowPaging="true" PageSize="10"
                                        onrowdeleting="GridView1_RowDeleting" Font-Size="14pt"
                                        onrowediting="GridView1_RowEditing"
                                        onrowcancelingedit="GridView1_RowCancelingEdit"
                                        onpageindexchanging="GridView1_PageIndexChanging"
                                        HorizontalAlign="Center"
                                        onrowupdating="GridView1_RowUpdating"
                                        onrowcommand="GridView1_RowCommand">
                <Columns>
                <%--Template for provider name --%>
                <asp:TemplateField HeaderText="Provider name" ItemStyle-Width="200">
                  <ItemTemplate>
                    <asp:Label id="myLabel1" runat="server" Text='<%# Bind("Name")%>' />
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:TextBox ID="myTextBox1" runat="server" Width="200" Text='<%# Bind("Name") %>'/>
                  </EditItemTemplate>
                  <FooterTemplate>
                    <asp:TextBox ID="myFooterTextBox1" runat="server" Width="200" Text='<%# Bind("Name") %>' />
                  </FooterTemplate>
                </asp:TemplateField>

                <%--Template for provider country--%>
                <asp:TemplateField HeaderText="Provider country" ItemStyle-Width="200">
                  <ItemTemplate>
                    <asp:Label id="myLabel2" runat="server" Text='<%# Bind("Country")%>' />
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:TextBox ID="myTextBox2" runat="server" Width="200" Text='<%# Bind("Country") %>'/>
                  </EditItemTemplate>
                  <FooterTemplate>
                    <asp:TextBox ID="myFooterTextBox2" runat="server" Width="200" Text='<%# Bind("Country") %>' />
                  </FooterTemplate>
                </asp:TemplateField>
                
                   <%--Commands--%>
                <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"> 
                  <ItemTemplate>
                    <asp:ImageButton ID="ibEdit" runat="server" CommandName="Edit" Text="Edit" ImageUrl="~/img/edit2.png" /> 
                    <asp:ImageButton ID="ibDelete" runat="server" CommandName="Delete" Text="Delete" ImageUrl="~/img/cross2.png" />
                    <asp:ImageButton ID="lbSelect" runat="server" CommandName="Select" ImageUrl="~/img/prod2.png" CommandArgument='<%# Container.DataItemIndex %>'/>
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:ImageButton ID="ibUpdate" runat="server" CommandName="Update" Text="Update" ImageUrl="~/img/edit2.png" />
                    <asp:ImageButton ID="ibCancel" runat="server" CommandName="Cancel" Text="Cancel" ImageUrl="~/img/cross2.png" />
                  </EditItemTemplate>
                  <FooterTemplate>
                    <asp:ImageButton ID="ibInsert" runat="server" CommandName="Insert" OnClick="ibInsert_Click" ImageUrl="~/img/add2.png" />
                  </FooterTemplate>
                 </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                  <table border="1" cellpadding="0" cellspacing="0">
                    <tr>
                      <td width="200" align="center">Provider name</td>
                      <td width="300" align="center">Provider country</td>
                      <td>Action</td>
                    </tr>
                    <tr>
                      <td>
                        <asp:TextBox ID="emptyProviderNameTextBox" runat="server" Width="200"/>
                      </td>
                      <td>
                        <asp:TextBox ID="emptyProviderCountryTextBox" runat="server" Width="300"/>
                      </td>
                      <td align="center">
                        <asp:ImageButton ID="emptyImageButton" runat="server" ImageUrl="~/img/prod2.png" OnClick="ibInsertInEmpty_Click"/>
                      </td>
                    </tr>
                   </table>
                 </EmptyDataTemplate>

                 <PagerStyle HorizontalAlign ="Center" />
            </asp:GridView>
           </td>
          </tr>
         </table>
</asp:Content>


