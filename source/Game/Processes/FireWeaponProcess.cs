using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;

namespace Game.Processes
{
    class FireWeaponProcess : Process
    {
        private Entity entity;
        private float firingSpeed;
        private float timeSinceLastShot;

        public FireWeaponProcess(Entity entity, float firingSpeed)
        {
            this.entity = entity;
            this.firingSpeed = firingSpeed;

            IsActive = true;
        }

        public override void OnInitialize() {
            timeSinceLastShot = firingSpeed;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            timeSinceLastShot += deltaTime;

            if (timeSinceLastShot >= firingSpeed) {
                timeSinceLastShot = 0f;
                Console.WriteLine("Firing Shot!");
                // TODO: Fire shot
            }
        }
    }
}
