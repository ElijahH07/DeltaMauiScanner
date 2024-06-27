namespace DeltaMauiScanner.Platforms.Android.ViewModels
{
    internal class RfidViewModel
    {
        public void UpdateRfidListView(string TagId)
        {

            var rfidTags = RFIDPage.Instance.RFIDTags;
            // Check if the tag ID already exists in rfidTags using OriginalTagId
            var existingTag = rfidTags.FirstOrDefault(tag => tag.OriginalTagId == TagId);

            if (existingTag != null) //checks if theres a copy
            {
                // +1 if tag exists in inventorhy
                //existingTag.Count++;
                //existingTag.TagId = "ID:" + TagId + " count:" + existingTag.Count;

                Console.WriteLine(existingTag.TagId); //edited

                //rfidTags.Add(existingTag);
            }
            else
            {
                //add new tag
                rfidTags.Add(new RFIDTag { OriginalTagId = TagId, TagId = "ID:" + TagId });  // + "  Count: 1", Count = 1 });
            }
        }
    }
}