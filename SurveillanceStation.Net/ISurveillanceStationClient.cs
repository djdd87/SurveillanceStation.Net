namespace SurveillanceStation.Net;

/// <summary>
/// Represents a client for interacting with the Synology Surveillance Station API.
/// </summary>
public interface ISurveillanceStationClient
{
    /// <summary>
    /// Authenticates the user and retrieves a session ID.
    /// </summary>
    /// <param name="account">The user account name.</param>
    /// <param name="password">The user account password.</param>
    /// <returns>The session ID as a string.</returns>
    Task LoginAsync(string account, string password);

    /// <summary>
    /// Logs out the current user session.
    /// </summary>
    Task LogoutAsync();

    /// <summary>
    /// Retrieves a list of cameras.
    /// </summary>
    /// <param name="keyword">Optional keyword to filter cameras.</param>
    /// <returns>A response containing a list of cameras.</returns>
    Task<CameraListResponse> ListCamerasAsync(string keyword = null);

    /// <summary>
    /// Edits the settings of a specific camera.
    /// </summary>
    /// <param name="camId">The ID of the camera to edit.</param>
    /// <param name="newName">Optional new name for the camera.</param>
    /// <param name="recordPrefix">Optional new record prefix.</param>
    /// <param name="recordSchedule">Optional new recording schedule.</param>
    /// <param name="rotationByDay">Optional new rotation by day setting.</param>
    /// <param name="rotationBySpace">Optional new rotation by space setting.</param>
    /// <returns>Updated camera information.</returns>
    Task<CameraInfo> EditCameraAsync(string camId, string newName = null, string recordPrefix = null, string recordSchedule = null, int? rotationByDay = null, int? rotationBySpace = null);

    /// <summary>
    /// Retrieves detailed information for specified cameras.
    /// </summary>
    /// <param name="camIds">Comma-separated list of camera IDs.</param>
    /// <param name="DIDOs">Whether to include digital input/output information.</param>
    /// <returns>An array of camera information.</returns>
    Task<CameraInfo[]> GetCameraInfoAsync(string camIds, bool DIDOs = false);

    /// <summary>
    /// Performs a PTZ (Pan-Tilt-Zoom) operation on a specified camera.
    /// </summary>
    /// <param name="camId">The ID of the camera to control.</param>
    /// <param name="action">The PTZ action to perform.</param>
    Task PerformPtzOperationAsync(string camId, string action);

    /// <summary>
    /// Controls the digital output of a specified camera.
    /// </summary>
    /// <param name="camId">The ID of the camera to control.</param>
    /// <param name="DOIndex">The index of the digital output to control.</param>
    /// <param name="triggerState">The state to set for the digital output.</param>
    /// <returns>Information about the controlled digital output.</returns>
    Task<DigitalOutputInfo> ControlDigitalOutputAsync(string camId, int DOIndex, bool triggerState);

    /// <summary>
    /// Downloads a recording for a specified camera and time range.
    /// </summary>
    /// <param name="camId">The ID of the camera.</param>
    /// <param name="startTime">The start time of the recording.</param>
    /// <param name="endTime">The end time of the recording.</param>
    /// <param name="fileName">Optional filename for the downloaded recording.</param>
    /// <param name="concate">Whether to concatenate multiple recordings if present.</param>
    /// <returns>The recording as a byte array.</returns>
    Task<byte[]> DownloadRecordingAsync(string camId, string startTime, string endTime, string fileName = null, bool concate = true);

    /// <summary>
    /// Downloads snapshots from a recording for a specified camera and time range.
    /// </summary>
    /// <param name="startTime">The start time for the snapshots.</param>
    /// <param name="endTime">The end time for the snapshots.</param>
    /// <param name="camId">The ID of the camera.</param>
    /// <param name="interval">The interval between snapshots in seconds.</param>
    /// <returns>The snapshots as a byte array.</returns>
    Task<byte[]> DownloadRecordingSnapshotsAsync(string startTime, string endTime, string camId, double interval);
    /// <summary>
    /// 
    /// Takes and downloads a snapshot from a specified camera.
    /// </summary>
    /// <param name="camId">The ID or name of the camera. This value will be automatically double-quoted in the request.</param>
    /// <param name="profileType">The profile type for the snapshot. (0: High quality, 1: Balanced)</param>
    /// <param name="save">Whether to also save the snapshot in Surveillance Station.</param>
    /// <param name="time">Optional specific time for the snapshot. This value will be automatically double-quoted in the request.</param>
    /// <returns>A byte array containing the JPEG image data of the snapshot.</returns>
    Task<byte[]> TakeAndDownloadSnapshotAsync(string camId, int profileType = 0, bool save = true, string time = null);

    /// <summary>
    /// Takes and saves a snapshot from a specified camera in Surveillance Station.
    /// </summary>
    /// <param name="camId">The ID or name of the camera. This value will be automatically double-quoted in the request.</param>
    /// <param name="profileType">The profile type for the snapshot. (0: High quality, 1: Balanced)</param>
    /// <param name="time">Optional specific time for the snapshot. This value will be automatically double-quoted in the request.</param>
    /// <returns>SnapshotInfo containing metadata about the taken snapshot.</returns>
    Task<SnapshotInfo> TakeAndSaveSnapshotAsync(string camId, int profileType = 0, string time = null);

    /// <summary>
    /// Creates a new bookmark.
    /// </summary>
    /// <param name="camId">The ID of the camera.</param>
    /// <param name="name">The name of the bookmark.</param>
    /// <param name="startTime">The start time of the bookmark.</param>
    /// <param name="endTime">Optional end time of the bookmark.</param>
    /// <param name="comment">Optional comment for the bookmark.</param>
    /// <returns>Information about the created bookmark.</returns>
    Task<BookmarkInfo> CreateBookmarkAsync(string camId, string name, string startTime, string endTime = null, string comment = null);

    /// <summary>
    /// Lists bookmarks based on specified criteria.
    /// </summary>
    /// <param name="camIds">Comma-separated list of camera IDs.</param>
    /// <param name="keyword">Optional keyword to filter bookmarks.</param>
    /// <param name="startTime">Optional start time to filter bookmarks.</param>
    /// <param name="endTime">Optional end time to filter bookmarks.</param>
    /// <returns>A response containing a list of bookmarks.</returns>
    Task<BookmarkListResponse> ListBookmarksAsync(string camIds, string keyword = null, string startTime = null, string endTime = null);

    /// <summary>
    /// Downloads a recording associated with a specific bookmark.
    /// </summary>
    /// <param name="bookmarkId">The ID of the bookmark.</param>
    /// <param name="dsId">The ID of the Diskstation.</param>
    /// <returns>The recording as a byte array.</returns>
    Task<byte[]> DownloadBookmarkRecordingAsync(int bookmarkId, string dsId = "0");

    /// <summary>
    /// Edits an existing bookmark.
    /// </summary>
    /// <param name="bookmarkId">The ID of the bookmark to edit.</param>
    /// <param name="name">Optional new name for the bookmark.</param>
    /// <param name="comment">Optional new comment for the bookmark.</param>
    /// <param name="startTime">Optional new start time for the bookmark.</param>
    /// <param name="endTime">Optional new end time for the bookmark.</param>
    /// <param name="dsId">The ID of the Diskstation.</param>
    /// <returns>Updated information about the bookmark.</returns>
    Task<BookmarkInfo> EditBookmarkAsync(int bookmarkId, string name = null, string comment = null, string startTime = null, string endTime = null, string dsId = "0");

    /// <summary>
    /// Deletes specified bookmarks.
    /// </summary>
    /// <param name="bookmarkIds">Comma-separated list of bookmark IDs to delete.</param>
    /// <param name="dsId">The ID of the Diskstation.</param>
    Task DeleteBookmarksAsync(string bookmarkIds, string dsId = "0");

    /// <summary>
    /// Updates the location of a device on the map.
    /// </summary>
    /// <param name="deviceId">The ID of the device to update.</param>
    /// <param name="longitude">Optional new longitude of the device.</param>
    /// <param name="latitude">Optional new latitude of the device.</param>
    /// <param name="radius">Optional new radius for the device's location.</param>
    /// <param name="viewAngle">Optional new view angle for the device.</param>
    /// <param name="direction">Optional new direction for the device.</param>
    /// <returns>Updated geographical information for the device.</returns>
    Task<GeoMapItem> UpdateDeviceLocationAsync(string deviceId, double? longitude = null, double? latitude = null, int? radius = null, int? viewAngle = null, int? direction = null);

    /// <summary>
    /// Creates a new license plate entry in the database.
    /// </summary>
    /// <param name="plateNumber">The license plate number.</param>
    /// <param name="type">The type of the license plate.</param>
    /// <param name="description">Optional description for the license plate.</param>
    /// <returns>Information about the created license plate entry.</returns>
    Task<LicensePlateInfo> CreateLicensePlateAsync(string plateNumber, int type = 1, string description = null);

    /// <summary>
    /// Lists license plates based on a keyword.
    /// </summary>
    /// <param name="keyword">Optional keyword to filter license plates.</param>
    /// <returns>A response containing a list of license plates.</returns>
    Task<LicensePlateListResponse> ListLicensePlatesAsync(string keyword = null);

    /// <summary>
    /// Edits an existing license plate entry.
    /// </summary>
    /// <param name="plateNumber">The current license plate number to edit.</param>
    /// <param name="newPlateNumber">Optional new license plate number.</param>
    /// <param name="type">Optional new type for the license plate.</param>
    /// <param name="description">Optional new description for the license plate.</param>
    /// <returns>Updated information about the license plate entry.</returns>
    Task<LicensePlateInfo> EditLicensePlateAsync(string plateNumber, string newPlateNumber = null, int? type = null, string description = null);

    /// <summary>
    /// Deletes specified license plates from the database.
    /// </summary>
    /// <param name="plateNumbers">Comma-separated list of license plate numbers to delete.</param>
    Task DeleteLicensePlatesAsync(string plateNumbers);

    /// <summary>
    /// Lists license plate events based on specified criteria.
    /// </summary>
    /// <param name="camIds">Optional comma-separated list of camera IDs.</param>
    /// <param name="plateNumbers">Optional comma-separated list of license plate numbers.</param>
    /// <param name="startTime">Optional start time to filter events.</param>
    /// <param name="endTime">Optional end time to filter events.</param>
    /// <param name="withThumbnail">Whether to include thumbnails in the response.</param>
    /// <param name="limit">Maximum number of events to return.</param>
    /// <returns>A response containing a list of license plate events.</returns>
    Task<LicensePlateEventListResponse> ListLicensePlateEventsAsync(string camIds = null, string plateNumbers = null, string startTime = null, string endTime = null, bool withThumbnail = false, int limit = 100);

    /// <summary>
    /// Downloads a recording associated with a specific license plate event.
    /// </summary>
    /// <param name="eventId">The ID of the license plate event.</param>
    /// <param name="dsId">The ID of the Diskstation.</param>
    /// <returns>The recording as a byte array.</returns>
    Task<byte[]> DownloadLicensePlateEventRecordingAsync(int eventId, string dsId = "0");

    /// <summary>
    /// Downloads a report of license plate events.
    /// </summary>
    /// <param name="camIds">Optional comma-separated list of camera IDs.</param>
    /// <param name="startTime">Optional start time for the report.</param>
    /// <param name="endTime">Optional end time for the report.</param>
    /// <param name="downloadFormat">The format of the downloaded report.</param>
    Task DownloadLicensePlateReportAsync(string camIds = null, string startTime = null, string endTime = null, int downloadFormat = 0);
}