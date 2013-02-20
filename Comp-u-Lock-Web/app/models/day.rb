class Day < ActiveRecord::Base
	attr_accessible :restriction_id, :hour_attributes

	has_many :days

	accepts_nested_attributes_for :hour
  	
  	def as_json options={}
    {
      id: id,
      restriction_id: restriction_id,

      hour_attributes: hour,

      created_at: created_at,
      update_at: updated_at
    }
  end
end
