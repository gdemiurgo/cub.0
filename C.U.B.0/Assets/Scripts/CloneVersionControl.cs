using UnityEngine;

public class CloneVersionControl : MonoBehaviour
{
	[SerializeField]
	private TextMesh cloneVersiontext;

	private int v1;

	private int v2;

	private void Start()
	{
		v1 = GameManager.instance.GetV1();
		v2 = GameManager.instance.GetV2();
		cloneVersiontext.text = v1 + "." + v2;
	}
}
