namespace UserSelectedNamespace
{
	public class _TempRefs : PrefabRefsGenerator.PrefabRefs
	{
		[UnityEngine.SerializeField]
		private TMPro.TMP_Text _text_1_tmp_t;
		public TMPro.TMP_Text text_1_tmp_t => _text_1_tmp_t;

		[UnityEngine.SerializeField]
		private TMPro.TMP_Text _text_2_tmp_t;
		public TMPro.TMP_Text text_2_tmp_t => _text_2_tmp_t;

		[UnityEngine.SerializeField]
		private UnityEngine.UI.Image _image_img;
		public UnityEngine.UI.Image image_img => _image_img;

#if UNITY_EDITOR
		private void OnValidate()
		{
			ValidateRef(ref _text_1_tmp_t, "Text 1 [tmp_t]");
			ValidateRef(ref _text_2_tmp_t, "Text 2 [tmp_t]");
			ValidateRef(ref _image_img, "Image [img]");
		}
#endif
	}
}
