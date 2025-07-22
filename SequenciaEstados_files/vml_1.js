
function ViewMgrSetVMLLocation(pageID, shapeID, pinX, pinY)
{
	doc = parent.frmDrawing.document;
	if (highlightDiv != null)
	{
		clickMenu ();

		var VMLImage = s;

		var imageLeft = 0;
		var imageRight = imageLeft + VMLImage.pixelWidth;
		var imageTop = 0;
		var imageBottom = imageTop + VMLImage.pixelHeight;

		var xLong = parent.ConvertXorYCoordinate(pinX, visBBoxLeft, visBBoxRight, imageLeft, imageRight, 0);
		var yLong = parent.ConvertXorYCoordinate(pinY, visBBoxBottom, visBBoxTop, imageTop, imageBottom, 1);

		xLong += doc.all['ConvertedImage'].style.posLeft;
		yLong += doc.all['ConvertedImage'].style.posTop;

		var arrowHalfWidth = viewMgr.highlightDiv.clientWidth / 2;
		var arrowHeight = viewMgr.highlightDiv.clientHeight;

		var boolNeedToScroll = false;

		if( !( (xLong - arrowHalfWidth) > doc.body.scrollLeft && (xLong + arrowHalfWidth) < (doc.body.scrollLeft + doc.body.clientWidth) ))
		{
			boolNeedToScroll = true;
		}
		
		if( !( (yLong - arrowHeight) > doc.body.scrollTop && (yLong + arrowHeight) < (doc.body.scrollTop + doc.body.clientHeight) ))
		{
			boolNeedToScroll = true;
		}
		
		if( boolNeedToScroll == true )
		{
			window.scrollTo( xLong - doc.body.clientWidth / 2, yLong - doc.body.clientHeight / 2);
		}
		
		highlightDiv.style.posLeft = xLong - arrowHalfWidth;
		highlightDiv.style.posTop = yLong;
		highlightDiv.style.visibility = "visible";

		setTimeout( "parent.hideObject(viewMgr.highlightDiv)", 200 );
		setTimeout( "parent.showObject(viewMgr.highlightDiv)", 400 );
		setTimeout( "parent.hideObject(viewMgr.highlightDiv)", 600 );
		setTimeout( "parent.showObject(viewMgr.highlightDiv)", 800 );
		setTimeout( "parent.hideObject(viewMgr.highlightDiv)", 1000 );
		setTimeout( "parent.showObject(viewMgr.highlightDiv)", 1200 );
		setTimeout( "parent.hideObject(viewMgr.highlightDiv)", 1400 );
		setTimeout( "parent.showObject(viewMgr.highlightDiv)", 1600 );
		setTimeout( "parent.hideObject(viewMgr.highlightDiv)", 1800 );

	}
}

function VMLZoomChange(size)
{
	if(size)
	{
		if(size == "up")
		{
			size = zoomLast + 50;
		}
		else if(size == "down")
		{
			size = zoomLast - 50;
		}
		
		size = parseInt(size);
		if(typeof(size) != "number")
			size = 100;
	}
	else
	{
		size = 100;
	}

	clickMenu ();

	viewMgr.zoomLast = size;

	var zoomFactor = size/100;

	var width = s.pixelWidth;
	var height = s.pixelHeight;

	var margin = parseInt(document.body.style.margin) * 2;

	var clientWidth = document.body.clientWidth;
	var clientHeight = document.body.clientHeight;

	var newScrollLeft = document.body.scrollLeft;
	var newScrollTop = document.body.scrollTop;

	var winwidth = clientWidth - margin;
	var winheight = clientHeight - margin;

	var widthRatio = winwidth / width;
	var heightRatio = winheight / height;

	if (widthRatio < heightRatio)
	{
		width = zoomFactor * winwidth;
		height = width / origWH;
	}
	else
	{
		height = zoomFactor * winheight;
		width = height * origWH;
	}

	s.pixelWidth = Math.max(width,1);
	s.pixelHeight = Math.max(height,1);

	sizeLast = size;

	var centerX = (zoomFactor / viewMgr.zoomFactor) * (newScrollLeft + (clientWidth / 2) - s.posLeft);
	var centerY = (zoomFactor / viewMgr.zoomFactor) * (newScrollTop + (clientHeight / 2) - s.posTop);

	viewMgr.zoomFactor = zoomFactor;

	if (width <= clientWidth)
	{
		s.posLeft = Math.max( 0, (clientWidth / 2) - (width / 2));
	}
	else
	{
		var left = centerX - (clientWidth / 2);
		if ( left >= 0 )
		{
			s.posLeft = 0;
			newScrollLeft = left;
		}
		else
		{
			s.posLeft = -left;
			newScrollLeft = 0;
		}
	}

	if (height <= clientHeight)
	{
		s.posTop = Math.max( 0, (clientHeight / 2) - (height / 2));
	}
	else
	{
		var top = centerY - (clientHeight / 2);
		if ( top >= 0 )
		{
			s.posTop = 0;
			newScrollTop = top;
		}
		else
		{
			s.posTop = -top;
			newScrollTop = 0;
		}
	}

	window.scrollTo(newScrollLeft, newScrollTop);

	s.visibility = "visible";

	var newXOffsetPercent = document.body.scrollLeft / s.pixelWidth;
	var newYOffsetPercent = document.body.scrollTop / s.pixelHeight;
	var newWidthPercent = document.body.clientWidth / s.pixelWidth;
	var newHeightPercent = document.body.clientHeight / s.pixelHeight;

	if (viewMgr.viewChanged)
	{
		viewMgr.viewChanged (newXOffsetPercent, newYOffsetPercent, newWidthPercent, newHeightPercent);
	}

	if (viewMgr.PostZoomProcessing)
	{
		viewMgr.PostZoomProcessing(size);
	}
}

function VMLSetView (xOffsetPercent, yOffsetPercent)
{
	var leftPixelOffset = xOffsetPercent * s.pixelWidth;
	var topPixelOffset = yOffsetPercent * s.pixelHeight;

	window.scrollTo (leftPixelOffset - s.posLeft, topPixelOffset - s.posTop);

	if (viewMgr.PostSetViewProcessing)
	{
		viewMgr.PostSetViewProcessing();
	}
}

function VMLOnResize ()
{
	if (viewMgr.zoomLast == 100)
	{
		viewMgr.Zoom(100);
	}

	if (viewMgr.viewChanged)
	{
		var image = document.all['ConvertedImage'];

		var newWidthPercent = document.body.clientWidth / image.style.pixelWidth;
		var newHeightPercent = document.body.clientHeight / image.style.pixelHeight;

		viewMgr.viewChanged (null, null, newWidthPercent, newHeightPercent);
	}
}

function VMLOnScroll ()
{
	if (viewMgr.viewChanged)
	{
		var image = document.all['ConvertedImage'];

		var newXOffsetPercent = document.body.scrollLeft / image.style.pixelWidth;
		var newYOffsetPercent = document.body.scrollTop / image.style.pixelHeight;

		viewMgr.viewChanged (newXOffsetPercent, newYOffsetPercent, null, null);
	}
}



