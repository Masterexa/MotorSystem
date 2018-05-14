using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NowhereUnity.Movement{


    [CreateAssetMenu(menuName="Scriptable/MotorCapacity")]
	public class MotorCapacity : ScriptableObject{

		#region Instance
			#region Fields
                public GeneralCapacityModel             general = new GeneralCapacityModel();
                public WalkingCapacityModel             walking = new WalkingCapacityModel();
                public AirborneCapacityModel            airborne = new AirborneCapacityModel();
                public OffMeshAnimationCapacityModel    offMeshAnimation = new OffMeshAnimationCapacityModel();
			#endregion

			#region Events
				// Use this for initialization
				void OnEnable() {
					
				}
			#endregion
		#endregion
	}
}