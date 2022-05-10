using PrefabRefsGenerator;

using UnityEngine;

using UserSelectedNamespace;

public class _Temp : PrefabRefsUser<_TempRefs>
{
	public void Start()
	{
		refs.text_1_tmp_t.text = "Nice ONE";
		refs.text_2_tmp_t.text = "Nice TWO";
		refs.image_img.color = Color.red;
	}
}