﻿using KeyPassBusiness;
using KeyPassInfoObjects;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyPassUserInterface
{
	public partial class GroupTreeControl : UserControl
	{

		#region privates
		public GroupTreeControl()
		{
			InitializeComponent();

			getGroups();

		}

		public void getGroups()
		{
			treeViewGroup.Nodes.Clear();
			List<Group> groups = DataManager.ListGroups();
			if (groups.Count <= 0)
			{
				UIContextManager.GroupSelected = null;
				return;
			}
			foreach (Group group in groups)
			{
				AddNodeByGroup(group);

			}
		}

		private void AddNodeByGroup(Group group)
		{
			TreeNode node = new TreeNode(group.Name);
			node.Tag = group;
			treeViewGroup.Nodes.Add(node);
			treeViewGroup.SelectedNode = node;
			treeViewGroup.Focus();
		}


		private void OnGroupSelected(object sender, TreeViewEventArgs e)
		{

			UpdateGroupSelectedUIManager((Group)e.Node.Tag);

		}

		private void UpdateGroupSelectedUIManager(Group group)
		{

			UIContextManager.GroupSelected = group;
		}


		private void onAddGroup(object sender, EventArgs e)
		{

			addGroupDialog();

		}

		private void OnEditGroup(object sender, EventArgs e)
		{


			editGroupDialog();

		}

		private void OnDeleteGroup(object sender, EventArgs e)
		{
			deleteGroup();
		}


		#endregion


		public void enableDisableStripItems(bool enable)
		{
			toolStripEditGroup.Enabled = _tsmEditGroup.Enabled = toolStripDeleteGroup.Enabled = _tmsDeleteGroup.Enabled = enable;

		}

		public void addGroupDialog()
		{
			AddEditGroupForm addEditGroupForm = new AddEditGroupForm();

			if (addEditGroupForm.ShowDialog() != DialogResult.OK)
				return;

			string groupName = addEditGroupForm.GroupName;

			Group group = DataManager.AddGroup(groupName);
			if (group != null)
			{
				AddNodeByGroup(group);
				UpdateGroupSelectedUIManager(group);

			}
		}

		public void editGroupDialog()
		{
			AddEditGroupForm addEditGroupForm = new AddEditGroupForm();
			string oldName = treeViewGroup.SelectedNode.Text;
			addEditGroupForm.GroupName = treeViewGroup.SelectedNode.Text;

			if (addEditGroupForm.ShowDialog() != DialogResult.OK)
				return;

			string newName = addEditGroupForm.GroupName;

			Group editedGroup = DataManager.EditGroup(oldName, newName);
			if (editedGroup != null)
			{
				treeViewGroup.SelectedNode.Text = newName;
			}
		}

		public void deleteGroup()
		{
			if (DataManager.DeleteGroup(treeViewGroup.SelectedNode.Text))
			{

				treeViewGroup.SelectedNode.Remove();
				if (treeViewGroup.Nodes.Count == 0)
				{
					UIContextManager.GroupSelected = null;
				}

			}
		}




	}
}
