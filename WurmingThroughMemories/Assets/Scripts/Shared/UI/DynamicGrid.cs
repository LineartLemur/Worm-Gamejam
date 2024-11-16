using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class DynamicGrid : MonoBehaviour {

	GridLayoutGroup _gridLayoutGroup;
	GridLayoutGroup gridLayoutGroup {
		get {
			if (!_gridLayoutGroup) _gridLayoutGroup = GetComponent<GridLayoutGroup>();
			return _gridLayoutGroup;
		}
	}

	RectTransform _rect;
	RectTransform rect {
		get {
			if (!_rect) _rect = GetComponent<RectTransform>();
			return _rect;
		}
	}

	[SerializeField]
	bool UseHeightRatio = false;

	[SerializeField]
	float heightRatio = 1.5f;	//Width * ratio for scalable height

	void Start() {
		UpdateCellSize();
	}

	void OnRectTransformDimensionsChange() {
		//print(rect.sizeDelta);
		//print(rect.rect.height + " " + rect.rect.width);

		UpdateCellSize();
	}

	void UpdateCellSize() {
		switch (gridLayoutGroup.constraint) {
			case GridLayoutGroup.Constraint.Flexible:
				break;
			case GridLayoutGroup.Constraint.FixedColumnCount:
				float height = gridLayoutGroup.cellSize.y;
				float width = rect.rect.width / gridLayoutGroup.constraintCount;
				//width -=  (gridLayoutGroup.padding.left / gridLayoutGroup.constraintCount) - (gridLayoutGroup.padding.right / gridLayoutGroup.constraintCount);
				width -=  gridLayoutGroup.spacing.x / gridLayoutGroup.constraintCount;

				if (UseHeightRatio) {
					height = width * heightRatio;
				}

				gridLayoutGroup.cellSize = new Vector2(width, height);
				break;
			case GridLayoutGroup.Constraint.FixedRowCount:
				break;
			default:
				break;
		}
	}
}
