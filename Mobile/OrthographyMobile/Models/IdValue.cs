using Newtonsoft.Json;

namespace OrthographyMobile.Models
{
	public class IdValue : BaseModel
	{
		[JsonIgnore]
		public static new string Route => "idvalues";
		public int LabelID { get; set; }
		[JsonIgnore]
		public bool Disabled { get; set; }
		[JsonIgnore]
		public string Label { get; set; }

		public IdValue()
		{
		}

		public IdValue(int id, int labelID)
		{
			ID = id;
			LabelID = labelID;
		}

		public override string ToString() => Label;
	}
}
