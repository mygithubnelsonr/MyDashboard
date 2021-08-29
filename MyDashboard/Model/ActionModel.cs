using System.Collections.Generic;

namespace MyDashboard.Model
{
    public class ActionModel
    {
        public class Rootobject
        {
            public int RowItems { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
            public Action[] actions { get; set; }
        }

        public class Action
        {
            public int ID { get; set; }
            public string Caption { get; set; }
            public string Image { get; set; }
            public string Start { get; set; }
            public string Command { get; set; }
        }
        public class ActionCollection
        {
            private List<Action> actions;

            public List<Action> Actions { get => actions; set => actions = value; }
        }
    }
}
