class Hour < ActiveRecord::Base
	attr_accessible :day_id, :start_time, :end_time

	validates :start, presence: true
	validates :end, presence: true
  	def as_json options={}
    {
      id: id,
      day_id: day_id,
      start_time: start_time,
  	  end_time: end_time,
  	  
      restriction_attributes: restriction,

      created_at: created_at,
      update_at: updated_at
    }
  end
end
