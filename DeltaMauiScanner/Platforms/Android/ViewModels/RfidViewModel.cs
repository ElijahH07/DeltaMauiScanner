namespace DeltaMauiScanner.Platforms.Android.ViewModels
{
    public class RfidViewModel
    {

        public void UpdateRfidListView(string TagId)
        {
            var rfidpage = RFIDPage.Instance;
            var rfidTags = rfidpage.RFIDTags;

            var existingTag = rfidTags.FirstOrDefault(tag => tag.OriginalTagId == TagId);
            if (existingTag != null)
            {
                existingTag.Count++;
                existingTag.Id = $"ID:{TagId}  Count:{existingTag.Count}";
                //Console.WriteLine("exists");
                int i = rfidTags.IndexOf(existingTag);
                rfidTags[i] = existingTag;
            }
            else
            {
                rfidTags.Add(new RFIDTag { OriginalTagId = TagId, Id = $"ID:{TagId}  Count:1", Count = 1 });
                //Console.WriteLine("new");
            }

            // Increment 'total' and synchronize with 'Globals.totalecount'
            Globals.totalecount++;
            rfidpage.SetTextForLabel(Globals.totalecount.ToString());
        }
    }
}