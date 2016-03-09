using UnityEngine;
using UnityEditor;
using NUnit.Framework;

using Monopoly.Model;
using Monopoly.Common;

public class UnitTest {

    [Test]
    public void EditorTest()
    {
        //Arrange
        var gameObject = new GameObject();
        //We register the newly create GameObject so it will be automatically removed after the test run
        Undo.RegisterCreatedObjectUndo (gameObject, "Created test GameObject");

        //Act
        //Try to rename the GameObject
        var newGameObjectName = "My game object";
        gameObject.name = newGameObjectName;

        //Assert
        //The object has a new name
        Assert.AreEqual(newGameObjectName, gameObject.name);
    }


	[Test]
	public void LoadGameBoardDataTest()
	{
		// load board JSON file as TextAsset.
		TextAsset boardText = Resources.Load<TextAsset>("JSON/board");

		// square game objects will be created in delegate event.
		LogicManager.instance.GameStart(boardText.text);

		Assert.AreEqual(LogicManager.instance.GetSquare(0).Type, Constants.SQ_GO);

		Assert.AreEqual(LogicManager.instance.GetSquare(10).Type, Constants.SQ_JAIL);

		// should fail here
		Assert.AreEqual(LogicManager.instance.GetSquare(10).Type, Constants.SQ_FREE);

	}

	[Test]
	public void LogicGameOverTest()
	{
		// load board JSON file as TextAsset.
		TextAsset boardText = Resources.Load<TextAsset>("JSON/board");

		// square game objects will be created in delegate event.
		LogicManager.instance.GameStart(boardText.text);

		LogicManager.instance.GameOver();

		Assert.AreEqual(LogicManager.instance.IsGameOver(), true);
	}


}
