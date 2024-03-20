
namespace tree_view_from_table
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "data.csv");
            foreach (var line in File.ReadAllLines(path))
            {
                string[] split = line.Split(',');
                if (!int.TryParse(split[0], out var _))
                {
                    continue; // Skip header
                }
                TreeNode? traverse = null;
                foreach (string token in split) 
                {
                    TreeNodeCollection? nodes = 
                        traverse is null ? 
                        treeView.Nodes : 
                        traverse.Nodes;
                    traverse = 
                        nodes
                        .OfType<TreeNode>()
                        .FirstOrDefault(_=>_.Text  == token);
                    if(traverse == null)
                    {
                        traverse = new TreeNode(token);
                        nodes.Add(traverse);
                    }
                }
            }
        }
    }
}
