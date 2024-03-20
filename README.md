## TreeView from (Delphi) Table

Your observation is correct. You have to begin with the "first" node and modify that node's collection of nodes, and go on down to the depth you need. So, to work with a tree view in Winforms, consider familiarizing yourself with [recursion](https://learn.microsoft.com/en-us/cpp/c-language/recursive-functions?view=msvc-170), [interation](https://learn.microsoft.com/en-us/dotnet/csharp/iterators) and (if you really want to make it as easy as possible) [System.Linq](https://learn.microsoft.com/en-us/dotnet/api/system.linq?view=net-8.0). 

___

_The sample data snippet you posted, assuming this is in CSV format_

```text
tdt_L1,tdt_L2,tdt_L3,tdt_L4
1102,1,0,0
1102,2,0,0
1102,3,0,0
1102,4,0,0
1102,5,0,0
1102,6,0,0
1102,7,0,0
1102,8,0,0
1103,0,0,0
1103,1,0,0
1103,2,0,0
1103,3,0,0
1103,4,0,0
1103,5,0,0
1103,6,0,0
1103,7,0,0
1103,8,0,0
```

**Method to read this .csv file**

For a `TreeView` named `treeView`, the root collection of `Nodes` is `treeView.Nodes`. 
```
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
```

**Result (unformatted)**

[![screenshot of tree][1]][1]

___

**Iterator**

If you then wanted to retrieve all of the tree nodes in a manner similar to the `Items` property you mentioned (essentially "flattening" the hierarchy) this can be done by making an extension method that uses recursion.

```
{ 
    ...
    foreach (var node in treeView.Descendants())
    {
        Debug.WriteLine(node.FullPath);
    }
    ...
}
```

###### Extension method

```
static partial class Extensions
{
    public static IEnumerable<TreeNode> Descendants(this TreeView treeView)
    {
        foreach (TreeNode root in treeView.Nodes)
        {
            foreach (var node in localTraverseNode(root))
            {
                yield return node;
            }
        }
        IEnumerable<TreeNode> localTraverseNode(TreeNode node)
        {
            yield return node;
            foreach (TreeNode child in node.Nodes)
            {
                foreach (var childNode in localTraverseNode(child))
                {
                    yield return childNode;
                }
            }
        }
    }
}
```


  [1]: https://i.stack.imgur.com/6bqAJ.png