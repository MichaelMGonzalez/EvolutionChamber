using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExperimentManager : MonoBehaviour {
	public float experimentTime = 60;
	public float timeScale = 1;
	public static int run = 0;
	private string sceneName;
	private Scene testingScene;
	void Start () {
		testingScene = SceneManager.GetActiveScene ();
		sceneName = testingScene.name;
		StartCoroutine (RunExperiment ());
	}

	IEnumerator RunExperiment() {
		while (true) {
			Time.timeScale = timeScale;
			yield return new WaitForSeconds (experimentTime);
			run++;
			Debug.Log (run);
			SceneManager.LoadScene (sceneName);
		}
	}

		
}
