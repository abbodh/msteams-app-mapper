namespace Microsoft.Teams.App.Mapper.Dialogs
{
    internal class KeywordQuery
    {
        private ClientContext ctx;

        public KeywordQuery(ClientContext ctx)
        {
            this.ctx = ctx;
        }

        public string QueryText { get; set; }
    }
}