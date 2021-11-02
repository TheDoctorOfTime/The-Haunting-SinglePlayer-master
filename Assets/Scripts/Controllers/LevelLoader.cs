using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	[SerializeField] Animator transitionAnimator;
	public void LoadScene(string sceneName)
	{
			StartCoroutine(LoadLevel(sceneName));
	}

	IEnumerator LoadLevel(string sceneName)
	{
		transitionAnimator.SetTrigger("LevelEnd");
		yield return new WaitForSeconds(.5f);
		SceneManager.LoadScene(sceneName);
	}
}
