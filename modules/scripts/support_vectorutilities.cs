/// relativeVectorToRotation by Xalos.
///
/// @param	for	3-element vector
/// @param	up	3-element vector
function relativeVectorToRotation(%for, %up)
{
    %yaw = mAtan(getWord(%for, 0), -getWord(%for, 1)) + $pi;    
	if(%yaw >= $pi) %yaw -= $m2pi;
	
    %rightAxis = vectorNormalize(vectorCross(%for, "0 0 1"));	
	if(vectorLen(%rightAxis) == 0) %rightAxis = "-1 0 0";
    
	%upAxis = vectorNormalize(vectorCross(%rightAxis, %for));
    
	%rDot = vectorDot(%up, %rightAxis);
    %uDot = vectorDot(%up, %upAxis);
    
    %euler = mAsin(vectorDot(vectorNormalize(%for), "0 0 1")) SPC mAtan(%rDot, %uDot) SPC %yaw;
	%matrix = MatrixCreateFromEuler(%euler);
    return getWords(%matrix, 3, 6);
}

/// getLookVector by Conan & Darksaber.
///
/// @param	this	player
function Player::getLookVector(%this)
{
	%forward = %this.getForwardVector();
	%eye = %this.getEyeVector();
	%up = %this.getUpVector();
	
	// Z-axis relative to the up vector.
	%magnitudeZ = vectorDot(%eye, %up);
	%localZ = vectorScale(%up, %magnitudeZ); 
	
	// Relative X/Y plane (eye vector minus relative Z-axis).
	%localPlane = vectorSub(%eye, %localZ); 
	
	// Y-axis relative to the forward vector.
	%magnitudeY = vectorDot(%localPlane, %forward); 
	%localY = vectorScale(%forward, %magnitudeY); 
	
	// Relative X-axis (X/Y plane minus relative Y-axis).
	%localX = vectorSub(%localPlane, %localY); 
	%magnitudeX = vectorLen(%localX);
	
	%magnitudePlane = mSqrt(%magnitudeX * %magnitudeX + %magnitudeY * %magnitudeY);
	
	return vectorAdd(vectorScale(%forward, %magnitudePlane), %localZ);
}